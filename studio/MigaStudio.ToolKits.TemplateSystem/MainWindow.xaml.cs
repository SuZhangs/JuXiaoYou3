using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Acorisoft.FutureGL.MigaUI;
using Acorisoft.FutureGL.MigaUI.Contracts;
using Acorisoft.FutureGL.MigaUI.Models;
using Acorisoft.FutureGL.MigaStudio.ToolKits.TemplateSystem;
using Acorisoft.FutureGL.MigaStudio.ToolKits.ViewModels;
using Acorisoft.FutureGL.MigaUI.Views;

namespace Acorisoft.FutureGL.MigaStudio.ToolKits
{
    class Provider : IBindingInfoProvider
    {
        public IEnumerable<BindingInfo> GetBindingInfo()
        {
            return new[]
            {
                new BindingInfo { ViewModel = typeof(ADialogViewModel), View = typeof(WarningView) },
                new BindingInfo { ViewModel = typeof(BDialogViewModel), View = typeof(BView) },
            };
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.UseWindowEventBroadcast();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel<AppViewModel>().Start();
        }

        private async void ShowA(object sender, RoutedEventArgs e)
        {
            await Dialog.ShowDialog(new StringViewModel()
            {
                Text  = Xaml.Lorem,
                Title = "Danger",
                
            }, null);
            await Dialog.ShowDialog(new DangerViewModel()
            {
                Content      = Xaml.Lorem,
                CountDown    = true,
                CountSeconds = 10,
                Title        = "Danger",
                CancelButtonText = Translate.Instance.CancelButtonText,
                CompleteButtonText = Translate.Instance.OkButtonText
            }, null);
            await Dialog.ShowDialog(new WarningViewModel()
            {
                Content = Xaml.Lorem,
                Title   = "Warning",
            }, null);
            await Dialog.ShowDialog(new InfoViewModel()
            {
                Content = Xaml.Lorem,
                Title   = "Info",
            }, null);
            await Dialog.ShowDialog(new SuccessViewModel()
            {
                Content = Xaml.Lorem,
                Title   = "Success",
            }, null);
            await Dialog.ShowDialog(new ObsoleteViewModel()
            {
                Content = Xaml.Lorem,
                Title   = "Obsolete",
            }, null);
        }

        private async void ShowB(object sender, RoutedEventArgs e)
        {
            var r = await Dialog.ShowDialog(new StringViewModel()
            {
                Text  = Xaml.Lorem,
                Title = "Danger",
            }, null);
            Debug.WriteLine($"Finished {r.Value}");
        }

        private void Button_ToggleIndicator(object sender, RoutedEventArgs e)
        {
            Dialog.IsBusy = !Dialog.IsBusy;
        }

        private void Button_Messaging(object sender, RoutedEventArgs e)
        {
            Dialog.Messaging(new IconMessage
            {
                Color    = "#99b433",
                Title    = "测试1",
                Delay    = TimeSpan.FromMilliseconds(1200),
                IsFilled = false,
                Geometry = Geometry.Parse("F1 M24,24z M0,0z M10.29,3.86L1.82,18A2,2,0,0,0,3.53,21L20.47,21A2,2,0,0,0,22.18,18L13.71,3.86A2,2,0,0,0,10.29,3.86z")
            });
            Dialog.Messaging(new IconMessage
            {
                Color    = "#BB1214",
                Title    = "操作错误：",
                Delay    = TimeSpan.FromMilliseconds(1200),
                IsFilled = false,
                Geometry = Geometry.Parse("F1 M24,24z M0,0z M10.29,3.86L1.82,18A2,2,0,0,0,3.53,21L20.47,21A2,2,0,0,0,22.18,18L13.71,3.86A2,2,0,0,0,10.29,3.86z")
            });
            Dialog.Messaging(new IconMessage
            {
                Color    = "#BB1214",
                Title    = "测试3",
                Delay    = TimeSpan.FromMilliseconds(1200),
                IsFilled = false,
                Geometry = Geometry.Parse("F1 M24,24z M0,0z M10.29,3.86L1.82,18A2,2,0,0,0,3.53,21L20.47,21A2,2,0,0,0,22.18,18L13.71,3.86A2,2,0,0,0,10.29,3.86z")
            });
        }
    }
}