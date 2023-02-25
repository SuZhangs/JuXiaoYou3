using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Basic
{
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    public class ChromeTabControl : Selector
    {
        private                bool                                           _addTabButtonClicked;
        private                object                                         _lastSelectedItem;
        private                Panel                                          _itemsHolder;
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
            ((ChromeTabPanel)ItemsHost).IsAddButtonEnabled = AddTabCommand.CanExecute(AddTabCommandParameter);
        }

        public static readonly DependencyProperty AddTabCommandParameterProperty =
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

        public static readonly DependencyProperty ReorderTabsCommandProperty =
            DependencyProperty.Register(
                nameof(ReorderTabsCommand),
                typeof(ICommand),
                typeof(ChromeTabControl));

        public ICommand ReorderTabsCommand
        {
            get => (ICommand)GetValue(ReorderTabsCommandProperty);
            set => SetValue(ReorderTabsCommandProperty, value);
        }

        // Provide CLR accessors for the event
        public event TabDragEventHandler TabDraggedOutsideBonds
        {
            add => AddHandler(TabDraggedOutsideBondsEvent, value);
            remove => RemoveHandler(TabDraggedOutsideBondsEvent, value);
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent TabDraggedOutsideBondsEvent = EventManager.RegisterRoutedEvent(
            "TabDraggedOutsideBonds", RoutingStrategy.Bubble, typeof(TabDragEventHandler), typeof(ChromeTabControl));


        // Provide CLR accessors for the event
        public event ContainerOverrideEventHandler ContainerItemPreparedForOverride
        {
            add => AddHandler(ContainerItemPreparedForOverrideEvent, value);
            remove => RemoveHandler(ContainerItemPreparedForOverrideEvent, value);
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent ContainerItemPreparedForOverrideEvent = EventManager.RegisterRoutedEvent(
            "ContainerItemPreparedForOverride", RoutingStrategy.Bubble, typeof(ContainerOverrideEventHandler), typeof(ChromeTabControl));


        [Obsolete("Set TabDragEventArgs.Handled in TabDraggedOutsideBonds event instead.")]
        public bool CloseTabWhenDraggedOutsideBonds
        {
            get => (bool)GetValue(CloseTabWhenDraggedOutsideBondsProperty);
            set => SetValue(CloseTabWhenDraggedOutsideBondsProperty, value);
        }

        // Using a DependencyProperty as the backing store for CloseTabWhenDraggedOutsideBonds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseTabWhenDraggedOutsideBondsProperty =
            DependencyProperty.Register(nameof(CloseTabWhenDraggedOutsideBonds), typeof(bool), typeof(ChromeTabControl), new PropertyMetadata(false));


        public bool IsAddButtonVisible
        {
            get => (bool)GetValue(IsAddButtonVisibleProperty);
            set => SetValue(IsAddButtonVisibleProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsAddButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAddButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsAddButtonVisible), typeof(bool), typeof(ChromeTabControl), new PropertyMetadata(true, IsAddButtonVisiblePropertyCallback));

        private static void IsAddButtonVisiblePropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            var ctc = d as ChromeTabControl;

            var panel = (ChromeTabPanel)ctc.ItemsHost;
            panel?.InvalidateVisual();
        }


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


        public Brush AddTabButtonBrush
        {
            get => (Brush)GetValue(AddButtonBrushProperty);
            set => SetValue(AddButtonBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddButtonBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonBrushProperty =
            DependencyProperty.Register(nameof(AddTabButtonBrush), typeof(Brush), typeof(ChromeTabControl), new PropertyMetadata(Brushes.Transparent));

        public Brush AddTabButtonMouseDownBrush
        {
            get => (Brush)GetValue(AddButtonMouseDownBrushProperty);
            set => SetValue(AddButtonMouseDownBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddButtonBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonMouseDownBrushProperty =
            DependencyProperty.Register(nameof(AddTabButtonMouseDownBrush), typeof(Brush), typeof(ChromeTabControl), new PropertyMetadata(Brushes.DarkGray));

        public Brush AddTabButtonMouseOverBrush
        {
            get => (Brush)GetValue(AddButtonMouseOverBrushProperty);
            set => SetValue(AddButtonMouseOverBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddButtonMouseOverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonMouseOverBrushProperty =
            DependencyProperty.Register(nameof(AddTabButtonMouseOverBrush), typeof(Brush), typeof(ChromeTabControl), new PropertyMetadata(Brushes.White));


        public Brush AddTabButtonDisabledBrush
        {
            get => (Brush)GetValue(AddButtonDisabledBrushProperty);
            set => SetValue(AddButtonDisabledBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddButtonDisabledBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonDisabledBrushProperty =
            DependencyProperty.Register(nameof(AddTabButtonDisabledBrush), typeof(Brush), typeof(ChromeTabControl), new PropertyMetadata(Brushes.DarkGray));


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


        /// <summary>
        /// Controls the persist behavior of tabs. All = all tabs live in memory, None = no tabs live in memory, Timed= tabs gets cleared from memory after a period of being unselected.
        /// </summary>
        public TabPersistBehavior TabPersistBehavior
        {
            get => (TabPersistBehavior)GetValue(TabPersistBehaviorProperty);
            set => SetValue(TabPersistBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for TabTearTriggerDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabPersistBehaviorProperty =
            DependencyProperty.Register(nameof(TabPersistBehavior), typeof(TabPersistBehavior), typeof(ChromeTabControl), new PropertyMetadata(TabPersistBehavior.None, OnTabPersistBehaviorPropertyChanged));

        private static void OnTabPersistBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ct = (ChromeTabControl)d;
            if (((TabPersistBehavior)e.NewValue) == TabPersistBehavior.None)
            {
                ct._itemsHolder.Children.Clear();
            }
            else
            {
                ct.SetSelectedContent(false);
            }
        }

        /// <summary>
        /// The time an inactive tab stays cached in memory before being cleared. Default duration is 30 minutes.
        /// </summary>
        public TimeSpan TabPersistDuration
        {
            get => (TimeSpan)GetValue(TabPersistDurationProperty);
            set => SetValue(TabPersistDurationProperty, value);
        }

        // Using a DependencyProperty as the backing store for TabTearTriggerDistance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabPersistDurationProperty =
            DependencyProperty.Register(nameof(TabPersistDuration), typeof(TimeSpan), typeof(ChromeTabControl), new PropertyMetadata(TimeSpan.FromMinutes(30)));


        public AddTabButtonBehavior AddTabButtonBehavior
        {
            get => (AddTabButtonBehavior)GetValue(AddTabButtonBehaviorProperty);
            set => SetValue(AddTabButtonBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddTabButtonBehavior.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddTabButtonBehaviorProperty =
            DependencyProperty.Register(nameof(AddTabButtonBehavior), typeof(AddTabButtonBehavior), typeof(ChromeTabControl), new PropertyMetadata(AddTabButtonBehavior.OpenNewTab));


        public ControlTemplate AddButtonTemplate
        {
            get => (ControlTemplate)GetValue(AddButtonTemplateProperty);
            set => SetValue(AddButtonTemplateProperty, value);
        }

        // Using a DependencyProperty as the backing store for AddButtonControlTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddButtonTemplateProperty =
            DependencyProperty.Register(nameof(AddButtonTemplate), typeof(ControlTemplate), typeof(ChromeTabControl), new PropertyMetadata(null, OnAddButtonTemplateChanged));

        private static void OnAddButtonTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctc = (ChromeTabControl)d;
            var panel = ctc.ItemsHost as ChromeTabPanel;
            panel?.SetAddButtonControlTemplate(e.NewValue as ControlTemplate);
        }

        public double AddTabButtonWidth
        {
            get => (double)GetValue(AddTabButtonWidthProperty);
            set => SetValue(AddTabButtonWidthProperty, value);
        }

        public static readonly DependencyProperty AddTabButtonWidthProperty =
            DependencyProperty.Register(nameof(AddTabButtonWidth), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(20.0));

        public double AddTabButtonHeight
        {
            get => (double)GetValue(AddTabButtonHeightProperty);
            set => SetValue(AddTabButtonHeightProperty, value);
        }

        public static readonly DependencyProperty AddTabButtonHeightProperty =
            DependencyProperty.Register(nameof(AddTabButtonHeight), typeof(double), typeof(ChromeTabControl), new PropertyMetadata(12.0));

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
            if (ItemsHost is ChromeTabPanel panel)
            {
                if (AddTabCommand != null)
                    panel.IsAddButtonEnabled = AddTabCommand.CanExecute(AddTabCommandParameter);
            }
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
                _addTabButtonClicked = true;
                AddTabCommand?.Execute(null);
            }
        }
        //internal void SetCanAddTab(bool value)
        //{
        //    SetValue(CanAddTabPropertyKey, value);
        //}
        //internal bool CanAddTab => (bool)GetValue(CanAddTabProperty);

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
            if (ReorderTabsCommand != null && ReorderTabsCommand.CanExecute(tabReorder))
            {
                ReorderTabsCommand.Execute(tabReorder);
            }
            else
            {
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
            _itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;
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
            if (_itemsHolder != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                    {
                        var itemsToRemove = _itemsHolder.Children.Cast<ContentPresenter>().Where(x => !Items.Contains(x.Content)).ToList();
                        foreach (var item in itemsToRemove)
                            _itemsHolder.Children.Remove(item);
                    }
                        break;
                    case NotifyCollectionChangedAction.Add:
                    {
                        // don't do anything with new items not created by the add button, because we don't want to
                        // create visuals that aren't being shown.
                        if (_addTabButtonClicked && AddTabButtonBehavior == AddTabButtonBehavior.OpenNewTab)
                        {
                            _addTabButtonClicked = false;
                            if (e.NewItems != null)
                                ChangeSelectedItem(AsTabItem(e.NewItems.Cast<object>().Last()));
                        }
                    }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems)
                            {
                                var cp = FindChildContentPresenter(item);
                                if (cp != null)
                                {
                                    _itemsHolder.Children.Remove(cp);
                                }
                            }
                        }


                        break;
                }
            }

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
            if (TabPersistBehavior != TabPersistBehavior.None)
            {
                if (item != null && _itemsHolder != null)
                {
                    CreateChildContentPresenter(SelectedItem);
                    // show the right child
                    foreach (ContentPresenter child in _itemsHolder.Children)
                    {
                        var childTabItem = AsTabItem(child.Content);
                        child.Visibility = childTabItem.IsSelected ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }

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

            RaiseEvent(new ContainerOverrideEventArgs(ContainerItemPreparedForOverrideEvent, this, item, AsTabItem(element)));
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

        /// <summary>
        /// create the child ContentPresenter for the given item (could be data or a TabItem)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ContentPresenter CreateChildContentPresenter(object item)
        {
            if (item == null)
            {
                return null;
            }

            var cp = FindChildContentPresenter(item);

            if (cp != null)
            {
                return cp;
            }

            // the actual child to be added. 
            cp = new ContentPresenter
            {
                Content    = (item is ChromeTabItem) ? (item as ChromeTabItem).Content : item,
                Visibility = Visibility.Collapsed
            };
            _itemsHolder.Children.Add(cp);
            return cp;
        }

        /// <summary>
        ///<para>Find the <see cref="ContentPresenter"/> for the given object. Data could be a <see cref="ChromeTabItem"/> or a ViewModel.</para>
        ///<para>Returns <see cref="ContentPresenter"/>, or <see langword="null" /> if the content is not loaded.</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is ChromeTabItem)
            {
                data = ((ChromeTabItem)data).Content;
            }

            if (data == null)
            {
                return null;
            }

            if (_itemsHolder == null)
            {
                return null;
            }

            foreach (ContentPresenter cp in _itemsHolder.Children)
            {
                if (cp.Content == data)
                {
                    return cp;
                }
            }

            return null;
        }

        internal void RemoveFromItemHolder(ChromeTabItem item)
        {
            if (_itemsHolder == null)
                return;
            var presenter = FindChildContentPresenter(item);
            if (presenter != null)
            {
                _itemsHolder.Children.Remove(presenter);
                Debug.WriteLine("Removing cached ContentPresenter");
            }
        }
    }
}