using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Modules.Views;
using Acorisoft.FutureGL.Tools.ModuleEditor.ViewModels;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.Tools.ModuleEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterServices(ILogger logger, IContainer container)
        { 
            
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            Xaml.InstallView(new BindingInfo
            {
                View = typeof(ModuleView),
                ViewModel = typeof(ModuleViewModel)
            });
        }

        protected override AppViewModel GetShell()
        {
            return new AppViewModel();
        }
    }
}