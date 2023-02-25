using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using DryIoc;
using NLog;
using Wpf.Ui.Styles.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Windows
{
    public abstract class TabApp<TViewModel, TMainController, TMainView> : ForestApp
        where TMainController : ITabViewController
        where TMainView : UserControl
        where TViewModel : TabBased
    {

        protected override (ILogger, ApplicationModel) RegisterFrameworkServices(IContainer container)
        {
            var r = base.RegisterFrameworkServices(container);

            Xaml.InstallView(new BindingInfo
            {
                View = typeof(TMainView),
                ViewModel = typeof(TMainController)
            });

            //
            // 创建Shell
            Xaml.Use<TViewModel>(GetShell());
            
            return r;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Controls;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });
            
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Forest.Shell;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });
            base.OnStartup(e);
        }


        protected abstract TViewModel GetShell();
    }

    public abstract class TabApp<TViewModel,TMainController, TMainView, TSplashController, TSplash> : TabApp<TViewModel,TMainController, TMainView>
        where TMainController : ITabViewController
        where TMainView : UserControl
        where TSplashController : IViewController
        where TSplash : UserControl
        where TViewModel : TabBased
    {
        
        protected override (ILogger, ApplicationModel) RegisterFrameworkServices(IContainer container)
        {
            var r = base.RegisterFrameworkServices(container);

            Xaml.InstallView(new BindingInfo
            {
                View      = typeof(TSplash),
                ViewModel = typeof(TSplashController)
            });
            
            return r;
        }
    }
}