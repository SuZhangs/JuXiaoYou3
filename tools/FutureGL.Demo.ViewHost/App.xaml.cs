using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using DryIoc;

namespace Acorisoft.FutureGL.Demo.ViewHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override (ILogger, ApplicationModel) RegisterFrameworkServices(IContainer container)
        {
            container.Use<ViewService, IViewService, IViewServiceAmbient>(new ViewService());
            return base.RegisterFrameworkServices(container);
        }

        protected override void RegisterServices(ILogger logger, IContainer container)
        {
            container.Use<DatabaseManager, IDatabaseManager>(DatabaseManager.GetDefaultDatabaseManager(logger));
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            SubSystem.InstallViews();
        }

    }
}