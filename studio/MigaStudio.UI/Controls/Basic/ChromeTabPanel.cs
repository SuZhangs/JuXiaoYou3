﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Acorisoft.FutureGL.MigaStudio.Utilities;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Basic
{
    [ToolboxItem(false)]
    public class ChromeTabPanel : Panel
    {
        private const    double           _stickyReanimateDuration = 0.10;
        private const    double           _tabWidthSlidePercent    = 0.5;
        private          bool             _isReleasingTab;
        private          Size             _finalSize;
        private readonly double           _leftMargin;
        private readonly double           _defaultMeasureHeight;
        private          double           _currentTabWidth;
        private          int              _captureGuard;
        private          int              _originalIndex;
        private          int              _slideIndex;
        private          List<double>     _slideIntervals;
        private          ChromeTabItem    _draggedTab;
        private          Point            _downPoint;
        private          Point            _downTabBoundsPoint;
        private          ChromeTabControl _parent;
        private          DateTime         _lastMouseDown;
        private readonly object           _lockObject = new object();

        protected double Overlap => ParentTabControl?.TabOverlap ?? 10;
        protected double MinTabWidth => _parent?.MinimumTabWidth ?? 40;
        protected double MaxTabWidth => _parent?.MaximumTabWidth ?? 125;
        protected double PinnedTabWidth => _parent?.PinnedTabWidth ?? MinTabWidth;

        static ChromeTabPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabPanel), new FrameworkPropertyMetadata(typeof(ChromeTabPanel)));
        }

        public ChromeTabPanel()
        {
            _leftMargin = 0.0;
            _defaultMeasureHeight = 30.0;
            var key = new ComponentResourceKey(typeof(ChromeTabPanel), "addButtonStyle");
            this.Loaded += ChromeTabPanel_Loaded;
            this.Unloaded += ChromeTabPanel_Unloaded;
        }

        private void ChromeTabPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) != null)
                Window.GetWindow(this).Activated -= ChromeTabPanel_Dectivated;
        }

        private void ChromeTabPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) != null)
                Window.GetWindow(this).Deactivated += ChromeTabPanel_Dectivated;
        }

        private void ChromeTabPanel_Dectivated(object sender, EventArgs e)
        {

            if (_draggedTab != null && !IsMouseCaptured && !_isReleasingTab)
            {
                var p = MouseUtilities.CorrectGetPosition(this);
                OnTabRelease(p, true, false);
            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            _currentTabWidth = CalculateTabWidth(finalSize);
            ParentTabControl.CanAddTabInternal=_currentTabWidth > MinTabWidth;

            _finalSize = finalSize;
            var offset = _leftMargin;
            
            foreach (UIElement element in Children)
            {
                var thickness = 0.0;
                var item = ItemsControl.ContainerFromElement(ParentTabControl, element) as ChromeTabItem;
                thickness = item.Margin.Bottom;
                var tabWidth = element.DesiredSize.Width;
                element.Arrange(new Rect(offset, 0, tabWidth, finalSize.Height - thickness));
                offset += tabWidth - Overlap;
            }
            
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _currentTabWidth = CalculateTabWidth(availableSize);
            ParentTabControl.CanAddTabInternal = _currentTabWidth > MinTabWidth;

            var height = double.IsPositiveInfinity(availableSize.Height) ? _defaultMeasureHeight : availableSize.Height;
            var resultSize = new Size(0, availableSize.Height);
            
            foreach (UIElement child in Children)
            {
                var item = ItemsControl.ContainerFromElement(ParentTabControl, child) as ChromeTabItem;
                var tabSize = new Size(GetWidthForTabItem(item), height - item.Margin.Bottom);
                child.Measure(tabSize);
                resultSize.Width += child.DesiredSize.Width - Overlap;
            }
            
            return resultSize;
        }
        private double GetWidthForTabItem(ChromeTabItem tab)
        {
            if (tab.IsPinned)
            {
                return PinnedTabWidth;
            }
            return _currentTabWidth;
        }

        private double CalculateTabWidth(Size availableSize)
        {
            var activeWidth = double.IsPositiveInfinity(availableSize.Width) ? 500 : availableSize.Width - _leftMargin;
            var numberOfPinnedTabs = Children.Cast<ChromeTabItem>().Count(x => x.IsPinned);

            var totalPinnedTabsWidth = numberOfPinnedTabs > 0 ? ((numberOfPinnedTabs * PinnedTabWidth)) : 0;
            var totalNonPinnedTabsWidth = ((activeWidth) + (Children.Count - 1) * Overlap) - totalPinnedTabsWidth;
            return Math.Min(Math.Max(totalNonPinnedTabsWidth / (Children.Count - numberOfPinnedTabs), MinTabWidth), MaxTabWidth);
        }



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SetTabItemsOnTabs();
            
            if (Children.Count > 0)
            {
                if (Children[0] is ChromeTabItem)
                    ParentTabControl.ChangeSelectedItem(Children[0] as ChromeTabItem);
            }
            
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            SetTabItemsOnTabs();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            lock (_lockObject)
            {
                if (_slideIntervals != null)
                {
                    return;
                }

                //Check if we clicked the close button, and return if we do.
                var originalSource = e.OriginalSource as DependencyObject;
                var isButton = false;
                while (true)
                {
                    if (originalSource != null && originalSource.GetType() != typeof(ChromeTabPanel))
                    {
                        var parent = VisualTreeHelper.GetParent(originalSource);
                        if (parent is Button)
                        {
                            isButton = true;
                            break;
                        }
                        originalSource = parent;
                    }
                    else
                        break;
                }
                if (isButton)
                    return;

                _downPoint = e.GetPosition(this);
                StartTabDrag(_downPoint);

            }
        }
        internal void StartTabDrag(ChromeTabItem tab = null, bool isTabGrab = false)
        {
            var downPoint = MouseUtilities.CorrectGetPosition(this);
            if (tab != null)
            {
                UpdateLayout();
                double totalWidth = 0;
                for (var i = 0; i < tab.Index; i++)
                {
                    totalWidth += GetWidthForTabItem(Children[i] as ChromeTabItem) - Overlap;
                }
                var xPos = totalWidth + ((GetWidthForTabItem(tab) / 2));
                _downPoint = new Point(xPos, downPoint.Y);
            }
            else
                _downPoint = downPoint;

            StartTabDrag(downPoint, tab, isTabGrab);
        }

        private ChromeTabItem GetTabFromMousePosition(Point mousePoint)
        {
            var source = GetVisualItemFromMousePosition(mousePoint);
            while (source != null && !Children.Contains(source as UIElement))
            {
                source = VisualTreeHelper.GetParent(source);
            }
            return source as ChromeTabItem;
        }
        private DependencyObject GetVisualItemFromMousePosition(Point mousePoint)
        {
            var result = VisualTreeHelper.HitTest(this, mousePoint);
            var source = result?.VisualHit;

            return source;
        }

        internal void StartTabDrag(Point p, ChromeTabItem tab = null, bool isTabGrab = false)
        {
            _lastMouseDown = DateTime.UtcNow;
            if (tab == null)
            {
                tab = GetTabFromMousePosition(_downPoint);
            }

            if (tab != null)
                _draggedTab = tab;
            else
            {
                //The mouse is not over a tab item, so just return.
                return;
            }

            if (_draggedTab != null)
            {
                if (Children.Count == 1
                    && ParentTabControl.DragWindowWithOneTab
                    && Mouse.LeftButton == MouseButtonState.Pressed
                    && !isTabGrab)
                {
                    _draggedTab = null;
                    Window.GetWindow(this).DragMove();
                }
                else
                {
                    _downTabBoundsPoint = MouseUtilities.CorrectGetPosition(_draggedTab);
                    SetZIndex(_draggedTab, 1000);
                    ParentTabControl.ChangeSelectedItem(_draggedTab);
                    if (isTabGrab)
                    {
                        ProcessMouseMove(new Point(p.X + 0.1, p.Y));
                    }
                }
            }
        }

        private void ProcessMouseMove(Point p)
        {
            var nowPoint = p;
            
            if (_draggedTab == null || !ParentTabControl.CanMoveTabs)
            {
                return;
            }

            var insideTabPoint = TranslatePoint(p, _draggedTab);
            var margin = new Thickness(nowPoint.X - _downPoint.X, 0, _downPoint.X - nowPoint.X, 0);


            var guardValue = Interlocked.Increment(ref _captureGuard);
            if (guardValue == 1)
            {
                _draggedTab.Margin = margin;

                //we capture the mouse and start tab movement
                _originalIndex = _draggedTab.Index;
                _slideIndex = _originalIndex + 1;
                //Add slide intervals, the positions  where the tab slides over the next.
                _slideIntervals = new List<double> { double.NegativeInfinity };

                for (var i = 1; i <= Children.Count; i += 1)
                {
                    var tab = Children[i - 1] as ChromeTabItem;
                    var diff = i - _slideIndex;
                    var sign = diff == 0 ? 0 : diff / Math.Abs(diff);
                    var bound = Math.Min(1, Math.Abs(diff)) * ((sign * GetWidthForTabItem(tab) * _tabWidthSlidePercent) + ((Math.Abs(diff) < 2) ? 0 : (diff - sign) * (GetWidthForTabItem(tab) - Overlap)));
                    _slideIntervals.Add(bound);
                }
                _slideIntervals.Add(double.PositiveInfinity);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (CaptureMouse())
                        Debug.WriteLine("has mouse capture=true");
                    else
                        Debug.WriteLine("has mouse capture=false");
                }));
            }
            else if (_slideIntervals != null)
            {
                if (insideTabPoint.X > 0 && (nowPoint.X + (_draggedTab.ActualWidth - insideTabPoint.X)) >= ActualWidth)
                {
                    return;
                }

                if (insideTabPoint.X < _downTabBoundsPoint.X && (nowPoint.X - insideTabPoint.X) <= 0)
                {
                    return;
                }
                _draggedTab.Margin = margin;
                //We return on small marging changes to avoid the tabs jumping around when quickly clicking between tabs.
                if (Math.Abs(_draggedTab.Margin.Left) < 10)
                    return;

                var changed = 0;
                var localSlideIndex = _slideIndex;
                if (localSlideIndex - 1 >= 0
                    && localSlideIndex - 1 < _slideIntervals.Count
                    && margin.Left < _slideIntervals[localSlideIndex - 1])
                {
                    SwapSlideInterval(localSlideIndex - 1);
                    localSlideIndex -= 1;
                    changed = 1;
                }
                else if (localSlideIndex + 1 >= 0
                    && localSlideIndex + 1 < _slideIntervals.Count
                    && margin.Left > _slideIntervals[localSlideIndex + 1])
                {
                    SwapSlideInterval(localSlideIndex + 1);
                    localSlideIndex += 1;
                    changed = -1;
                }
                if (changed != 0)
                {
                    var rightedOriginalIndex = _originalIndex + 1;
                    var diff = 1;
                    if (changed > 0 && localSlideIndex >= rightedOriginalIndex)
                    {
                        changed = 0;
                        diff = 0;
                    }
                    else if (changed < 0 && localSlideIndex <= rightedOriginalIndex)
                    {
                        changed = 0;
                        diff = 2;
                    }

                    var index = localSlideIndex - diff;
                    if (index >= 0 && index < Children.Count)
                    {
                        var shiftedTab = Children[index] as ChromeTabItem;

                        if (!shiftedTab.Equals(_draggedTab)
                            && ((shiftedTab.IsPinned && _draggedTab.IsPinned) || (!shiftedTab.IsPinned && !_draggedTab.IsPinned)))
                        {
                            var offset = changed * (GetWidthForTabItem(_draggedTab) - Overlap);
                            StickyReanimate(shiftedTab, offset, _stickyReanimateDuration);
                            _slideIndex = localSlideIndex;
                        }
                    }
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (_draggedTab != null && Mouse.LeftButton != MouseButtonState.Pressed && !_isReleasingTab)
            {
                var p = e.GetPosition(this);
                OnTabRelease(p, true, false);
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            ProcessMouseMove(e.GetPosition(this));
        }


        private void OnTabRelease(Point p, bool isDragging, bool closeTabOnRelease, double animationDuration = _stickyReanimateDuration)
        {
            lock (_lockObject)
            {
                
                if (isDragging)
                {
                    ReleaseMouseCapture();
                    var offset = GetTabOffset();
                    var localSlideIndex = _slideIndex;
                    void completed()
                    {
                        if (_draggedTab != null)
                        {
                            try
                            {
                                ParentTabControl.ChangeSelectedItem(_draggedTab);
                                var vm = _draggedTab.Content;
                                _draggedTab.Margin = new Thickness(offset, 0, -offset, 0);
                                _draggedTab = null;
                                _captureGuard = 0;
                                ParentTabControl.MoveTab(_originalIndex, localSlideIndex - 1);
                                _slideIntervals = null;
                                InvalidateVisual();
                                if (closeTabOnRelease && ParentTabControl.CloseTabCommand != null)
                                {
                                    Debug.WriteLine("sendt close tab command");
                                    ParentTabControl.CloseTabCommand.Execute(vm);
                                }
                                if (Children.Count > 1)
                                {
                                    //this fixes a bug where sometimes tabs got stuck in the wrong position.
                                    RealignAllTabs();
                                }
                            }
                            finally
                            {
                                _isReleasingTab = false;
                            }
                        }
                    }

                    if (Reanimate(_draggedTab, offset, animationDuration, completed))
                    {
                        _isReleasingTab = true;
                    }
                }
                else
                {
                    if (_draggedTab != null)
                    {
                        var offset = GetTabOffset();
                        ParentTabControl.ChangeSelectedItem(_draggedTab);
                        _draggedTab.Margin = new Thickness(offset, 0, -offset, 0);
                    }
                    _draggedTab = null;
                    _captureGuard = 0;
                    _slideIntervals = null;
                }
            }
        }

        private double GetTabOffset()
        {
            double offset = 0;
            if (_slideIntervals != null)
            {
                if (_slideIndex < _originalIndex + 1)
                {
                    offset = _slideIntervals[_slideIndex + 1] - GetWidthForTabItem(_draggedTab) * (1 - _tabWidthSlidePercent) + Overlap;
                }
                else if (_slideIndex > _originalIndex + 1)
                {
                    offset = _slideIntervals[_slideIndex - 1] + GetWidthForTabItem(_draggedTab) * (1 - _tabWidthSlidePercent) - Overlap;
                }
            }
            return offset;
        }

        private void RealignAllTabs()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                var shiftedTab = Children[i] as ChromeTabItem;
                var offset = 1 * (GetWidthForTabItem(shiftedTab) - Overlap);
                shiftedTab.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            OnTabRelease(e.GetPosition(this), IsMouseCaptured, false);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            _parent = null;

        }

        private ChromeTabControl ParentTabControl
        {
            get
            {
                if (_parent == null)
                {
                    DependencyObject parent = this;
                    while (parent != null && !(parent is ChromeTabControl))
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }
                    _parent = parent as ChromeTabControl;
                }
                return _parent;
            }
        }
        /*
         Unused method => 
        private UIElement GetTopContainer() => Application.Current.MainWindow.Content as UIElement;
        */
        private void StickyReanimate(ChromeTabItem tab, double left, double duration)
        {
            void completed()
            {
                if (_draggedTab != null)
                {
                    tab.Margin = new Thickness(left, 0, -left, 0);
                }
            }

            Reanimate(tab, left, duration, completed);
        }

        private bool Reanimate(ChromeTabItem tab, double left, double duration, Action completed)
        {
            if (tab == null)
            {
                return false;
            }
            var offset = new Thickness(left, 0, -left, 0);
            var moveBackAnimation = new ThicknessAnimation(tab.Margin, offset, new Duration(TimeSpan.FromSeconds(duration)));
            Storyboard.SetTarget(moveBackAnimation, tab);
            Storyboard.SetTargetProperty(moveBackAnimation, new PropertyPath(MarginProperty));
            var sb = new Storyboard();
            sb.Children.Add(moveBackAnimation);
            sb.FillBehavior = FillBehavior.Stop;
            sb.AutoReverse = false;
            sb.Completed += (o, ea) =>
            {
                sb.Remove();
                completed?.Invoke();
            };
            sb.Begin();
            return true;
        }



        private void SetTabItemsOnTabs()
        {
            for (var i = 0; i < Children.Count; i += 1)
            {
                if (!(Children[i] is DependencyObject depObj))
                {
                    continue;
                }
                var item = ItemsControl.ContainerFromElement(ParentTabControl, depObj) as ChromeTabItem;
                if (item != null)
                {
                    KeyboardNavigation.SetTabIndex(item, i);
                }
            }
        }

        private void SwapSlideInterval(int index)
        {
            _slideIntervals[_slideIndex] = _slideIntervals[index];
            _slideIntervals[index] = 0;
        }
    }
}