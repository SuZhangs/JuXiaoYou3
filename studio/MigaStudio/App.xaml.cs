using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ForestApp
    {
        private const string RepositorySettingFileName = "repo.json";
        private const string AdvancedSettingFileName   = "advanced.json";
        
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

        protected override (ILogger, ApplicationModel) RegisterFrameworkServices(IContainer container)
        {
            var data = base.RegisterFrameworkServices(container);
            
            //
            //
            var (logger, appModel) = data;
            
            
            //
            // Repository Setting
            var repositorySettingFileName = Path.Combine(appModel.Settings, RepositorySettingFileName);
            var repositorySetting = JSON.OpenSetting<RepositorySetting>(repositorySettingFileName,
                () => new RepositorySetting
                {
                    LastRepository = null,
                    Repositories = new HashSet<RepositoryCache>()
                });

            //
            // 注册设置
            Xaml.Use<ISystemSetting>(new SystemSetting
            {
                RepositorySetting = repositorySetting,
                RepositorySettingFileName = repositorySettingFileName
            });

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