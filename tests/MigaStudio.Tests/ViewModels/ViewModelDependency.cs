using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.MigaDB.Core;
using DryIoc;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MigaStudio.Tests.ViewModels
{
    public class ViewModelDependency
    {
        private static ILogger ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var debugFileTarget = new DebuggerTarget
            {
                Layout = "【${level}】 ${date:HH:mm:ss} ${message}"
            };
            
            var logfile = new FileTarget("logfile")
            {
                FileName = "${shortdate}.log",
                Layout   = "${level} ${shortdate} | ${message}  ${exception} ${event-properties:myProperty}"
            };

            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugFileTarget);

            LogManager.Configuration = config;
            return LogManager.GetLogger("App");
        }
        
        public static void Initialize(IContainer container)
        {
            var logger   = ConfigureLogger();
            

            //
            // 注册服务
            container.Use<ForestResourceFactory, ITextResourceFactory>(new ForestResourceFactory());
            container.Use<WindowEventBroadcast, IWindowEventBroadcast, IWindowEventBroadcastAmbient>(new WindowEventBroadcast());
            container.Use<DialogService,
                IDialogService,
                IDialogServiceAmbient,
                IBusyService,
                IBusyServiceAmbient,
                INotifyServiceAmbient,
                INotifyService,
                IBuiltinDialogService>(new DialogService());
            container.RegisterInstance(typeof(ILogger), logger);
            container.Use<DatabaseManager, IDatabaseManager>(
                DatabaseManager.GetDefaultDatabaseManager(
                    logger,
                    DatabaseMode.Debug));
        }
    }
}