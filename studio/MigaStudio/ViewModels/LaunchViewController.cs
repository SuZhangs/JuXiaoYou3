using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewController : LaunchViewControllerBase
    {
        private const string RepositorySettingFileName = "repo.json";
        private const string AdvancedSettingFileName   = "advanced.json";

        public LaunchViewController()
        {
            // 加载设置
            Job(StringFromCode.GetText("text.launch.loadSetting"), _ =>
            {
                var appModel = Xaml.Get<ApplicationModel>();

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
                var advancedSetting = JSON.OpenSetting<AdvancedSettingModel>(repositorySettingFileName,
                    () => new AdvancedSettingModel
                    {
                        DebugMode = DebugMode.Release,
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
            });
            
            // 检查更新
            Job(StringFromCode.GetText("text.launch.checkVersion"), x =>
            {
                Thread.Sleep(1000);
            });
            
            Job(StringFromCode.GetText("text.launch.openDatabase"), x =>
            {
                Thread.Sleep(1000);
            });
        }

        public override void Start(Parameter arg)
        {
            Context = arg.Args[0] as GlobalStudioContext;
            base.Start(arg);
        }

        protected override object GetExecuteContext() => Context;

        protected override void OnJobCompleted()
        {
            //
            //
            Context.SwitchController(Context.MainController);
        }
        
        public GlobalStudioContext Context { get; private set; }
    }
}