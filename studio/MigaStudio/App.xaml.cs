﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        
        protected sealed override ApplicationModel ConfigureDirectory()
        {
            var domain = ApplicationModel.CheckDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "JuXiaoYou"));

            return new ApplicationModel
            {
                Logs     = Path.Combine(domain, "Logs"),
                Settings = Path.Combine(domain, "UserData")
            }.Initialize();
        }

        protected override AppViewModel GetShell()
        {
            return new AppViewModel();
        }

        protected override (ILogger, ApplicationModel) RegisterFrameworkServices(IContainer container)
        {
            var data = base.RegisterFrameworkServices(container);
            
            //
            //
            var (logger, appModel) = data;
            

            container.Use<ViewServiceAdapter,
                IViewServiceAdapter,
                IViewServiceAmbient>(new ViewServiceAdapter());

            return data;
        }

        protected override void RegisterServices(ILogger logger, IContainer container)
        {
            container.Use<DatabaseManager, IDatabaseManager>(DatabaseManager.GetDefaultDatabaseManager(logger));
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
        }
    }
}