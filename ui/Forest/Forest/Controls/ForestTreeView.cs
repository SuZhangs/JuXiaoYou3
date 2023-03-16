using System.Windows;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestTreeView : TreeView
    {
        static ForestTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTreeView), new FrameworkPropertyMetadata(typeof(ForestTreeView)));
        }
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ForestTreeViewItem();
        }


        public static readonly DependencyProperty BindableSelectedItemProperty = DependencyProperty.Register(
            nameof(BindableSelectedItem),
            typeof(object),
            typeof(ForestTreeView),
            new PropertyMetadata(default(object)));

        public object BindableSelectedItem
        {
            get => (object)GetValue(BindableSelectedItemProperty);
            set => SetValue(BindableSelectedItemProperty, value);
        }
    }

    public class ForestTreeViewItem : TreeViewItem
    {
        static ForestTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTreeViewItem), new FrameworkPropertyMetadata(typeof(ForestTreeViewItem)));
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(ForestTreeViewItem),
            new PropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(ForestTreeViewItem),
            new PropertyMetadata(Boxing.False));

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            nameof(Placement),
            typeof(Dock),
            typeof(ForestTreeViewItem),
            new PropertyMetadata(Dock.Left));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(ForestTreeViewItem),
            new PropertyMetadata(17d));

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}