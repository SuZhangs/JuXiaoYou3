using System.Windows.Data;

namespace Acorisoft.FutureGL.Forest
{
    public class XamlAssist : DependencyObject
    {
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached(
            "GroupNameOverride", 
            typeof(string), 
            typeof(XamlAssist), 
            new PropertyMetadata(default(string)));

        public static void SetGroupName(ItemsControl element, string value)
        {
            element.SetValue(GroupNameProperty, value);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            
            element.Loaded += OnApplyGroupStyle;
        }

        private static void OnApplyGroupStyle(object sender, RoutedEventArgs e)
        {
            var i   = (ItemsControl)sender;
            var cvs = CollectionViewSource.GetDefaultView(i.ItemsSource);

            if (cvs?.GroupDescriptions is null)
            {
                return;
            }
            
            cvs.GroupDescriptions.Clear();
            cvs.GroupDescriptions.Add(new PropertyGroupDescription(GetGroupName(i)));
        }

        public static string GetGroupName(ItemsControl element)
        {
            return (string)element.GetValue(GroupNameProperty);
        }
    }
}