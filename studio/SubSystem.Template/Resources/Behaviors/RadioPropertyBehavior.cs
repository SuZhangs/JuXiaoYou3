using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using Microsoft.Xaml.Behaviors;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Behaviors
{
    public class RadioPropertyBehavior : Behavior<RadioButton>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject is RadioButton { DataContext: OptionItemUI ui })
            {
                AssociatedObject.IsChecked = ui.SelectedValue == ui.Value;
            }
            
            AssociatedObject.Checked += OnChecked;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Checked -= OnChecked;
            base.OnDetaching();
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject is not RadioButton { DataContext: OptionItemUI ui })
            {
                return;
            }

            ui.Expression?.Invoke(ui);
        }
    }
}