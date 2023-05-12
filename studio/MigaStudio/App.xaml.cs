using System.IO;
using System.Windows;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.UI;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Services;
using DryIoc;
using NLog;
// ReSharper disable UnusedParameter.Local

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
            // TemplateSystem.InstallViews();
        }


        protected override void OnExitOverride(ExitEventArgs e)
        {
            Xaml.Get<AppViewModel>()
                .Stop();
            //
            // 移除所有对象
            Task.Run(async () => await _databaseManager.CloseAsync())
                .GetAwaiter()
                .GetResult();
        }
    }
}