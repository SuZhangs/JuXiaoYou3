﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Basic
{
    public class ChromeTabItem : HeaderedContentControl
    {
        private DispatcherTimer _persistentTimer;

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ChromeTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnIsSelectedChanged));


        public bool IsPinned
        {
            get => (bool)GetValue(IsPinnedProperty);
            set => SetValue(IsPinnedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsPinned.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPinnedProperty =
            DependencyProperty.Register(nameof(IsPinned), typeof(bool), typeof(ChromeTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));



        public Brush SelectedTabBrush
        {
            get => (Brush)GetValue(SelectedTabBrushProperty);
            set => SetValue(SelectedTabBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedTabBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTabBrushProperty =
            DependencyProperty.Register(nameof(SelectedTabBrush), typeof(Brush), typeof(ChromeTabItem), new PropertyMetadata(Brushes.White));



        private static readonly RoutedUICommand _closeTabCommand = new RoutedUICommand("Close tab", "CloseTab", typeof(ChromeTabItem));

        public static RoutedUICommand CloseTabCommand => _closeTabCommand;

        private static readonly RoutedUICommand _closeAllTabsCommand = new RoutedUICommand("Close all tabs", "CloseAllTabs", typeof(ChromeTabItem));

        public static RoutedUICommand CloseAllTabsCommand => _closeAllTabsCommand;

        private static readonly RoutedUICommand _closeOtherTabsCommand = new RoutedUICommand("Close other tabs", "CloseOtherTabs", typeof(ChromeTabItem));

        public static RoutedUICommand CloseOtherTabsCommand => _closeOtherTabsCommand;

        private static readonly RoutedUICommand _pinTabCommand = new RoutedUICommand("Pin Tab", "PinTab", typeof(ChromeTabItem));

        public static RoutedUICommand PinTabCommand => _pinTabCommand;

        public static void SetIsSelected(DependencyObject item, bool value)
        {
            item.SetValue(IsSelectedProperty, value);
        }

        public static bool GetIsSelected(DependencyObject item)
        {
            return (bool)item.GetValue(IsSelectedProperty);
        }

        private ChromeTabControl ParentTabControl => ItemsControl.ItemsControlFromItemContainer(this) as ChromeTabControl;

        static ChromeTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabItem), new FrameworkPropertyMetadata(typeof(ChromeTabItem)));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeTabItem), new CommandBinding(_closeTabCommand, HandleCloseTabCommand));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeTabItem), new CommandBinding(_closeAllTabsCommand, HandleCloseAllTabsCommand));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeTabItem), new CommandBinding(_closeOtherTabsCommand, HandleCloseOtherTabsCommand));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeTabItem), new CommandBinding(_pinTabCommand, HandlePinTabCommand));

        }
        public ChromeTabItem()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.Loaded += ChromeTabItem_Loaded;
        }

        private void ChromeTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ChromeTabItem_Loaded;
            this.Unloaded += ChromeTabItem_Unloaded;
        }

        private void ChromeTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= ChromeTabItem_Unloaded;
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        private static void HandlePinTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is ChromeTabItem item)) { return; }
            item.ParentTabControl.PinTab(item.DataContext);
        }


        private static void HandleCloseOtherTabsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is ChromeTabItem item)) { return; }
            item.ParentTabControl.RemoveAllTabs(item.DataContext);
        }

        private static void HandleCloseAllTabsCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is ChromeTabItem item)) { return; }
            item.ParentTabControl.RemoveAllTabs();
        }

        private static void HandleCloseTabCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is ChromeTabItem item)) { return; }
            item.ParentTabControl.RemoveTab(item);
        }
        public int Index => ParentTabControl?.GetTabIndex(this) ?? -1;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Return)
            {
                ParentTabControl.ChangeSelectedItem(this);
            }
        }
    }
}