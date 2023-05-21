using System.IO;
using System.Linq;
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

        public static void SynchronizeSetting()
        {
            var db = Xaml.Get<IDatabaseManager>();
            if (db.IsOpen
                  .CurrentValue)
            {
                var p = db.Property
                          .CurrentValue;

                if (p is null)
                {
                    return;
                }
                var id = p.Id;
                var ss = Xaml.Get<SystemSetting>();
                var rs = ss.RepositorySetting;
                var r = rs.Repositories
                          .FirstOrDefault(x => x.Id == id);

                if (r is null)
                {
                    return;
                }

                r.Author = p.Author;
                r.Name   = p.Name;
                r.Intro  = p.Intro;
                
                //
                // 移除所有对象
                Task.Run(async () => await ss.SaveAsync())
                    .GetAwaiter()
                    .GetResult();
            }

        }

        protected override void OnExitOverride(ExitEventArgs e)
        {
            SynchronizeSetting();
            
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