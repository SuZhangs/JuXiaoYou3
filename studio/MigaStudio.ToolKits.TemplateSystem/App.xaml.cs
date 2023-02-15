using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.MigaUI;
using Acorisoft.FutureGL.MigaUI.ViewModels;
using DryIoc;
using Acorisoft.FutureGL.MigaStudio.ToolKits.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ToolKits
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// 创建Shell
        /// </summary>
        /// <returns></returns>
        protected override AppViewModelBase CreateShell() => new AppViewModel();

        protected override void RegisterServices(IContainer container)
        {
        }

        protected override void RegisterViews(IViewInstaller installer)
        {
            installer.Install(new Provider());
            Xaml.GetIoc().UseBuiltinViews();
        }

        protected override void RegisterDependentViews(IViewInstaller installer)
        {
        }
    }
}