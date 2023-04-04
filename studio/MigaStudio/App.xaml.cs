using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.UI;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Services;
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
        private const string SettingDir                = "JuXiaoYou";
        private const string UserDataDir               = "UserData";
        private const string LogDir                    = "Logs";
        private const string BasicSettingFileName      = "juxiaoyou-main.json";
        private const string RepositorySettingFileName = "repo.json";
        private const string AdvancedSettingFileName   = "advanced.json";

        private IDatabaseManager _databaseManager;

        public App() : base(BasicSettingFileName)
        {
            Current.DispatcherUnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.Error(e.Exception);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ExceptionObject);
        }

        protected sealed override ApplicationModel ConfigureDirectory()
        {
            var domain = ApplicationModel.CheckDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    SettingDir));

            return new ApplicationModel
            {
                Logs     = Path.Combine(domain, LogDir),
                Settings = Path.Combine(domain, UserDataDir)
            }.Initialize();
        }

        protected override AppViewModel GetShell()
        {
            return new AppViewModel();
        }


        protected override void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            var setting = InstallSetting(logger, appModel, container);
            
            //
            // TODO: 安装语言
            SubSystem.InstallLanguages();
            TemplateSystem.InstallLanguages();
            
            _databaseManager = container.Use<DatabaseManager, IDatabaseManager>(
                DatabaseManager.GetDefaultDatabaseManager(
                    logger,
                    setting.DebugMode));
            
            // _databaseManager = container.Use<DatabaseManager, IDatabaseManager>(
            //     DatabaseManager.GetDefaultDatabaseManager(
            //         logger,
            //         DatabaseMode.Debug));

            container.Use<AutoSaveService, IAutoSaveService>(new AutoSaveService());
            container.RegisterInstance<MusicService>(new MusicService());
        }

        private static AdvancedSettingModel InstallSetting(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            logger.Info("写入设置");
            
            //
            // Repository Setting
            var repositorySettingFileName = Path.Combine(appModel.Settings, RepositorySettingFileName);
            var repositorySetting = JSON.OpenSetting<RepositorySetting>(repositorySettingFileName,
                () => new RepositorySetting
                {
                    LastRepository = null,
                    Repositories   = new HashSet<RepositoryCache>()
                });

            //
            // Repository Setting
            var advancedSettingFileName = Path.Combine(appModel.Settings, AdvancedSettingFileName);
            var advancedSetting = JSON.OpenSetting<AdvancedSettingModel>(advancedSettingFileName,
                () => new AdvancedSettingModel
                {
                    DebugMode = DatabaseMode.Release,
                });

            //
            // 注册设置
            Xaml.Use<SystemSetting, ISystemSetting>(new SystemSetting
            {
                AdvancedSettingFileName   = advancedSettingFileName,
                AdvancedSetting           = advancedSetting,
                RepositorySetting         = repositorySetting,
                RepositorySettingFileName = repositorySettingFileName
            });
            
            return advancedSetting;
        }

        protected override void RegisterResourceDictionary(ResourceDictionary appResDict)
        {
            appResDict.MergedDictionaries.Add(ForestUI.UseToolKits());
            base.RegisterResourceDictionary(appResDict);
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            //
            // TODO: 安装视图
            SubSystem.InstallViews();
            TemplateSystem.InstallViews();
        }


        protected override async void OnExitOverride(ExitEventArgs e)
        {
            //
            // 移除所有对象
            await _databaseManager.CloseAsync();
        }
    }
}