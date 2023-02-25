using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Basic
{
    public class ChromeTabControl : Selector
    {
        private                object                                         _lastSelectedItem;
        private                ConditionalWeakTable<object, DependencyObject> _objectToContainerMap;
        public static readonly DependencyProperty                             SelectedContentProperty = DependencyProperty.Register(nameof(SelectedContent), typeof(object), typeof(ChromeTabControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        internal bool CanAddTabInternal { get; set; }

        public static readonly DependencyProperty CloseTabCommandProperty =
            DependencyProperty.Register(
                nameof(CloseTabCommand),
                typeof(ICommand),
                typeof(ChromeTabControl));

        public ICommand CloseTabCommand
        {
            get => (ICommand)GetValue(CloseTabCommandProperty);
            set => SetValue(CloseTabCommandProperty, value);
        }

        public static readonly DependencyProperty PinTabCommandProperty =
            DependencyProperty.Register(
                nameof(PinTabCommand),
                typeof(ICommand),
                typeof(ChromeTabControl));

        public ICommand PinTabCommand
        {
            get => (ICommand)GetValue(PinTabCommandProperty);
            set => SetValue(PinTabCommandProperty, value);
        }

        public static readonly DependencyProperty AddTabCommandProperty =
            DependencyProperty.Register(
                nameof(AddTabCommand),
                typeof(ICommand),
                typeof(ChromeTabControl), new PropertyMetadata(AddTabCommandPropertyChanged));

        private static void AddTabCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            var ct = (ChromeTabControl)d;
            if (e.NewValue != null)
            {
                var command = (ICommand)e.NewValue;
                command.CanExecuteChanged += ct.Command_CanExecuteChanged;
            }

            if (e.OldValue != null)
            {
                var command = (ICommand)e.OldValue;
                command.CanExecuteChanged -= ct.Command_CanExecuteChanged;
            }
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            
            AddTabCommand.CanExecute(AddTabCommandParameter);
        }

        public static DependencyProperty AddTabCommandParameterProperty =
            DependencyProperty.Register(nameof(AddTabCommandParameter), typeof(object), typeof(ChromeTabControl));

        public object AddTabCommandParameter
        {
            get => GetValue(AddTabCommandParameterProperty);
            set => SetValue(AddTabCommandParameterProperty, value);
        }

        public ICommand AddTabCommand
        {
            get => (ICommand)GetValue(AddTabCommandProperty);
            set => SetValue(AddTabCommandProperty, value);
        }

        [Obsolete("Set TabDragEventArgs.Handled in TabDraggedOutsideBonds event instead.")]
        public bool CloseTabWhenDraggedOutsideBonds
        {
            get => (bool)GetValue(CloseTabWhenDraggedOutsideBondsProperty);
            set => SetValue(CloseTabWhenDraggedOutsideBondsProperty, value);
        }

        // Using a DependencyProperty as the backing store for CloseTabWhenDraggedOutsideBonds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseTabWhenDraggedOutsideBondsProperty =
            DependencyProperty.Register(nameof(CloseTabWhenDraggedOutsideBonds), typeof(bool), typeof(ChromeTabControl), new PropertyMetadata(false));


        public bool CanMoveTabs
        {
            get => (bool)GetValue(CanMoveTabsProperty);
            set => SetValue(CanMoveTabsProperty, value);
        }

        // Using a DependencyProperty as the backing store for CanMoveTabs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanMoveTabsProperty =
            DependencyProperty.Register(nameof(CanMoveTabs), typeof(bool), typeof(ChromeTabControl), new PropertyMetadata(true));


        public bool DragWindowWithOneTab
        {
            get => (bool)GetValue(DragWindowWithOneTabProperty);
            set => SetValue(DragWindowWithOneTabProperty, value);
        }

        // Using a DependencyProperty as the backing store for DragWindowWithOneTab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragWindowWithOneTabProperty =
            DependencyProperty.Register(nameof(DragWindowWithOneTab), typeof(bool), typeof(ChromeTabControl), new PropertyMetadata(true));


        public Brush SelectedTabBrush
        {
            get => (Brush)GetValue(SelectedTabBrushProperty);
            set => SetValue(SelectedTabBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedTabBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTabBrushProperty =
            DependencyProperty.Register(nameof(SelectedTabBrush), typeof(Brush), typeof(ChromeTabControl), new PropertyMetadata(null, SelectedTabBrushPropertyCallback));

        public double MinimumTabWidth
        {
            get => (double)GetValue(MinimumTabWidthProperty);
            set => SetValue(MinimumTabWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinimumTabWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumTabWidthProperty =
            DependencyProperty.Register(nameof(MinimumTabWidth), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(40.0, OnMinimumTabWidthPropertyChanged));

        private static void OnMinimumTabWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctc = (ChromeTabControl)d;
            ctc.CoerceValue(PinnedTabWidthProperty);
            ctc.CoerceValue(MaximumTabWidthProperty);
        }

        public double MaximumTabWidth
        {
            get => (double)GetValue(MaximumTabWidthProperty);
            set => SetValue(MaximumTabWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MaximumTabWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumTabWidthProperty =
            DependencyProperty.Register(nameof(MaximumTabWidth), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(125.0, null, OnCoerceMaximumTabWidth));

        private static object OnCoerceMaximumTabWidth(DependencyObject d, object baseValue)
        {
            var ctc = (ChromeTabControl)d;

            if ((double)baseValue <= ctc.MinimumTabWidth)
                return ctc.MinimumTabWidth + 1;
            return baseValue;
        }

        public double PinnedTabWidth
        {
            get => (double)GetValue(PinnedTabWidthProperty);
            set => SetValue(PinnedTabWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for PinnedTabWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PinnedTabWidthProperty =
            DependencyProperty.Register(nameof(PinnedTabWidth), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(40.0, null, OnCoercePinnedTabWidth));

        private static object OnCoercePinnedTabWidth(DependencyObject d, object baseValue)
        {
            var ctc = (ChromeTabControl)d;

            if (ctc.MinimumTabWidth > (double)baseValue)
                return ctc.MinimumTabWidth;
            return baseValue;
        }

        /// <summary>
        /// The extra pixel distance you need to drag up or down the tab before the tab tears out.
        /// </summary>
        public double TabTearTriggerDistance
        {
            get => (double)GetValue(TabTearTriggerDistanceProperty);
            set => SetValue(TabTearTriggerDistanceProperty, value);
        }

        // Using a DependencyProperty as the backing store for TabTearTriggerDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabTearTriggerDistanceProperty =
            DependencyProperty.Register(nameof(TabTearTriggerDistance), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(0.0));

        public double TabOverlap
        {
            get => (double)GetValue(TabOverlapProperty);
            set => SetValue(TabOverlapProperty, value);
        }

        // Using a DependencyProperty as the backing store for TabOverlap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabOverlapProperty =
            DependencyProperty.Register(nameof(TabOverlap), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(10.0));

        static ChromeTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabControl), new FrameworkPropertyMetadata(typeof(ChromeTabControl)));
        }

        public ChromeTabControl()
        {
            Loaded += ChromeTabControl_Loaded;
        }

        private void ChromeTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddTabCommand?.CanExecute(AddTabCommandParameter);
        }

        private static void SelectedTabBrushPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctc = (ChromeTabControl)d;
            if (e.NewValue != null && ctc.SelectedItem != null)
                ctc.AsTabItem(ctc.SelectedItem).SelectedTabBrush = (Brush)e.NewValue;
        }

        /// <summary>
        /// Grabs hold of the tab based on the input viewmodel and positions it at the mouse cursor.
        /// </summary>
        /// <param name="viewModel"></param>
        public void GrabTab(object viewModel)
        {
            var p = (ChromeTabPanel)ItemsHost;
            var item = AsTabItem(viewModel);
            p.StartTabDrag(item, true);
        }

        protected Panel ItemsHost => (Panel)typeof(MultiSelector).InvokeMember("ItemsHost",
            BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance,
            null, this, null);

        internal void AddTab()
        {
            if (!CanAddTabInternal)
            {
                return;
            }

            if (AddTabCommand != null && AddTabCommand.CanExecute(null))
            {
                AddTabCommand?.Execute(null);
            }
        }

        internal void RemoveTab(object obj)
        {
            var removeItem = AsTabItem(obj);
            if (CloseTabCommand != null && CloseTabCommand.CanExecute(removeItem.DataContext))
            {
                CloseTabCommand.Execute(removeItem.DataContext);
                RemoveFromItemHolder(removeItem);
            }
        }

        internal void PinTab(object tab)
        {
            var removeItem = AsTabItem(tab);
            if (PinTabCommand != null && PinTabCommand.CanExecute(removeItem.DataContext))
            {
                PinTabCommand.Execute(removeItem.DataContext);
            }
        }

        internal void RemoveAllTabs(object exceptThis = null)
        {
            var objects = ItemsSource.Cast<object>().Where(x => x != exceptThis).ToList();
            foreach (var obj in objects)
            {
                if (CloseTabCommand != null && CloseTabCommand.CanExecute(obj))
                {
                    CloseTabCommand.Execute(obj);
                }
            }
        }


        public object SelectedContent
        {
            get => GetValue(SelectedContentProperty);
            set => SetValue(SelectedContentProperty, value);
        }

        internal int GetTabIndex(ChromeTabItem item)
        {
            for (var i = 0; i < Items.Count; i += 1)
            {
                var tabItem = AsTabItem(Items[i]);
                if (Equals(tabItem, item))
                {
                    return i;
                }
            }

            return -1;
        }

        internal void ChangeSelectedIndex(int index)
        {
            if (Items.Count <= index)
                return;
            var item = AsTabItem(Items[index]);
            ChangeSelectedItem(item);
        }

        internal void ChangeSelectedItem(ChromeTabItem item)
        {
            var index = GetTabIndex(item);
            if (index != SelectedIndex)
            {
                if (index > -1)
                {
                    if (SelectedItem != null)
                    {
                        Panel.SetZIndex(AsTabItem(SelectedItem), 0);
                    }

                    SelectedIndex = index;
                    Panel.SetZIndex(item, 1001);
                }
            }

            if (SelectedContent == null && item != null)
                SetSelectedContent(false);
        }

        internal void MoveTab(int fromIndex, int toIndex)
        {
            if (Items.Count == 0
                || fromIndex == toIndex
                || fromIndex >= Items.Count
                || fromIndex < 0
                || toIndex >= Items.Count
                || toIndex < 0)
            {
                return;
            }

            var fromTab = Items[fromIndex];
            var toTab = Items[toIndex];
            var fromItem = AsTabItem(fromTab);
            var toItem = AsTabItem(toTab);
            if (fromItem.IsPinned && !toItem.IsPinned)
                return;
            if (!fromItem.IsPinned && toItem.IsPinned)
                return;
            
            var tabReorder = new TabReorder(fromIndex, toIndex);
            
            var sourceType = ItemsSource.GetType();
            if (sourceType.IsGenericType)
            {
                var sourceDefinition = sourceType.GetGenericTypeDefinition();
                if (sourceDefinition == typeof(ObservableCollection<>))
                {
                    var method = sourceType.GetMethod("Move");
                    method.Invoke(ItemsSource, new object[] { fromIndex, toIndex });
                }
            }

            for (var i = 0; i < Items.Count; i += 1)
            {
                var v = AsTabItem(Items[i]);
                v.Margin = new Thickness(0);
            }

            SelectedItem = fromTab;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetSelectedContent(false);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var tab = new ChromeTabItem();
            if (SelectedTabBrush != null)
                tab.SelectedTabBrush = SelectedTabBrush;
            return tab;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ChromeTabItem);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SetInitialSelection();
            KeyboardNavigation.SetIsTabStop(this, false);
        }

        protected void SetInitialSelection()
        {
            bool? somethingSelected = null;
            foreach (var element in Items)
            {
                if (element is DependencyObject o)
                    somethingSelected |= ChromeTabItem.GetIsSelected(o);
            }

            if (somethingSelected.HasValue && somethingSelected.Value == false)
            {
                SelectedIndex = 0;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            SetSelectedContent(Items.Count == 0);
            SetChildrenZ();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SetChildrenZ();


            SetSelectedContent(e.AddedItems.Count == 0);
        }

        protected void SetSelectedContent(bool removeContent)
        {
            if (removeContent)
            {
                if (SelectedItem == null)
                {
                    if (Items.Count > 0)
                    {
                        SelectedItem = _lastSelectedItem ?? Items[0];
                    }
                    else
                    {
                        SelectedItem    = null;
                        SelectedContent = null;
                    }
                }

                return;
            }

            if (SelectedIndex > 0)
            {
                _lastSelectedItem = Items[SelectedIndex - 1];
            }
            else if (SelectedIndex == 0 && Items.Count > 1)
            {
                _lastSelectedItem = Items[SelectedIndex + 1];
            }
            else
            {
                _lastSelectedItem = null;
            }

            var item = AsTabItem(SelectedItem);
            SelectedContent = item?.Content;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element != item)
            {
                ObjectToContainer.Remove(item);
                ObjectToContainer.Add(item, element);
                SetChildrenZ();
            }
            
            var tabItem = AsTabItem(element);
            var pinned = (item as IPinnable)?.IsPinned ?? false;
            tabItem.IsPinned = pinned;
        }

        protected ChromeTabItem AsTabItem(object item)
        {
            var tabItem = item as ChromeTabItem;
            if (tabItem == null && item != null)
            {
                DependencyObject dp;
                ObjectToContainer.TryGetValue(item, out dp);
                tabItem = dp as ChromeTabItem;
            }

            return tabItem;
        }

        private ConditionalWeakTable<object, DependencyObject> ObjectToContainer => 
            _objectToContainerMap ??= new ConditionalWeakTable<object, DependencyObject>();

        protected void SetChildrenZ()
        {
            var zindex = Items.Count - 1;
            foreach (var element in Items)
            {
                var tabItem = AsTabItem(element);
                if (tabItem == null)
                {
                    continue;
                }

                if (ChromeTabItem.GetIsSelected(tabItem))
                {
                    Panel.SetZIndex(tabItem, Items.Count);
                }
                else
                {
                    Panel.SetZIndex(tabItem, zindex);
                }

                zindex -= 1;
            }
        }

        internal void RemoveFromItemHolder(ChromeTabItem item)
        {
        }
    }
}