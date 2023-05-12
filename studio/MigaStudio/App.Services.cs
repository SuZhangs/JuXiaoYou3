using System.Drawing;
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

namespace Acorisoft.FutureGL.MigaStudio
{
    partial class App
    {
        protected override void RegisterServices(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            var setting    = InstallSetting(logger, appModel, container);
            _databaseManager = container.Use<DatabaseManager, IDatabaseManager>(DatabaseManager.GetDefaultDatabaseManager(logger, setting.DebugMode));
            var attachable = new InMemoryServiceHost(container, _databaseManager);
            
            //
            // 注册数据库附加服务
            InstallInMemoryService(attachable);
            InstallAutoSaveService(logger, setting, container);
            
            container.RegisterInstance<MusicService>(new MusicService());
        }

        private static void InstallAutoSaveService(ILogger logger, AdvancedSettingModel setting, IContainer container)
        {
            var autoSave = new AutoSaveService();
            var period   = Math.Clamp(setting.AutoSavePeriod, 1, 10);
            autoSave.Elapsed = period;
            logger.Info($"设置自动保存服务，自动保存时间为：{period}");
            

            //
            // 注册服务
            container.Use<AutoSaveService, IAutoSaveService>(autoSave);
        }
        
        private static AdvancedSettingModel InstallSetting(ILogger logger, ApplicationModel appModel, IContainer container)
        {
            logger.Info("正在读取设置...");
            
            //
            // Repository Setting
            var repositorySettingFileName = Path.Combine(appModel.Settings, RepositorySettingFileName);
            var repositorySetting = JSON.OpenSetting<RepositorySetting>(repositorySettingFileName,
                () => new RepositorySetting
                {
                    LastRepository = null,
                    Repositories   = new ObservableCollection<RepositoryCache>()
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
            container.Use<SystemSetting, ISystemSetting>(new SystemSetting
            {
                AdvancedSettingFileName   = advancedSettingFileName,
                AdvancedSetting           = advancedSetting,
                RepositorySetting         = repositorySetting,
                RepositorySettingFileName = repositorySettingFileName
            });
            
            return advancedSetting;
        }
    }
}