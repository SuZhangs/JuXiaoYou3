using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            DataContext = new AppViewModel();
            InitializeComponent();
            Xaml.Get<IWindowEventBroadcastAmbient>().SetEventSource(this);
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement { Tag: Type type } fe)
            {
                return;
            }

            var dialog = (IDialogViewModel)Activator.CreateInstance(type)!;
            dialog.Title = type.Name;

            if (dialog is OperationViewModel dialog2)
            {
                dialog2.CompleteButtonText = Forest.Language.ConfirmText;
                dialog2.CancelButtonText = Forest.Language.CancelText;
            }
            
            await DialogHost.ShowDialog(dialog, null);
        }
    }
}