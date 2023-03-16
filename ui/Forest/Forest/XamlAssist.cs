namespace Acorisoft.FutureGL.Forest
{
    public  class XamlAssist : DependencyObject
    {

        public static readonly DependencyProperty ItemProperty = DependencyProperty.RegisterAttached(
            "Item", typeof(object), typeof(XamlAssist), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty EnableBindingProperty = DependencyProperty.RegisterAttached(
            "EnableBindingOverride", typeof(bool), typeof(XamlAssist), new PropertyMetadata(default(bool)));

        public static void SetEnableBinding(DependencyObject element, bool value)
        {
            if (element is TreeView tv)
            {
                OnAttached(tv);
                tv.Unloaded += (o, _) => OnDetaching(o as TreeView);
            }
            element.SetValue(EnableBindingProperty, value);
        }

        public static bool GetEnableBinding(DependencyObject element)
        {
            return (bool)element.GetValue(EnableBindingProperty);
        }
        
        static void OnAttached(TreeView AssociatedObject)
        {
            AssociatedObject.SelectedItemChanged  += OnSelectedItemChanged;
            AssociatedObject.MouseRightButtonDown += OnMouseRightButtonDown;
        }

        static void OnDetaching(TreeView AssociatedObject)
        {
            AssociatedObject.SelectedItemChanged  -= OnSelectedItemChanged;
            AssociatedObject.MouseRightButtonDown -= OnMouseRightButtonDown;
        }

        private static void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ancestor = Xaml.FindAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            
            if(ancestor is null)return;
            
            ancestor.IsSelected = true;
        }

        private static void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is not TreeView AssociatedObject)
            {
                return;
            }

            SetItem(AssociatedObject, AssociatedObject.SelectedItem);
        }

        public static void SetItem(DependencyObject element, object value)
        {
            element.SetValue(ItemProperty, value);
        }

        public static object GetItem(DependencyObject element)
        {
            return (object)element.GetValue(ItemProperty);
        }
    }
}