using System.Reactive.Concurrency;
using System.Reflection;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using DryIoc;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MigaStudio.Tests.Core
{
    public static class ViewModelUnitTestArchitecture
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
        
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //
            // 初始化
            ViewModelUnitTestArchitecture.Initialize(Xaml.Container);
        }

        public static void Initialize(IContainer container)
        {
            var logger = ConfigureLogger();


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
            container.RegisterInstance<IScheduler>(CurrentThreadScheduler.Instance);
            container.RegisterInstance<ILogger>(logger);
            var dbMgr = container.Use<DatabaseManager, IDatabaseManager>(
                DatabaseManager.GetDefaultDatabaseManager(
                    logger,
                    DatabaseMode.Debug));

            dbMgr.LoadAsync("C:\\")
                 .GetAwaiter()
                 .GetResult();
        }
        
        public class PropertyNullable
        {
            public string Name { get; init; }
            public bool Result { get; init; }
            public Type PropertyType { get; init; }
            public override string ToString()
            {
                return $"{Name}: {Result}";
            }
        }

        public static void UnitTest(ViewModelBase vm)
        {
            ConstructorUnitTest(vm);
            LifetimeControl(vm);
        }
        
        private static void LifetimeControl(IViewModel vm)
        {
            vm.Start();
            vm.Start();
            vm.Start();
            vm.Suspend();
            vm.Suspend();
            vm.Suspend();
            vm.Resume();
            vm.Resume();
            vm.Resume();
            vm.Stop();
            vm.Stop();
            vm.Stop();
        }

        private static void ConstructorUnitTest(ViewModelBase vm)
        {
            AssertAllPropertyWasNotNull(vm);
            AssertAllFieldWasNotNull(vm);
        }
        
        public static void AssertAllPropertyWasNotNull(object value)
        {
            if (value is null)
            {
                Assert.Fail(nameof(value));
            }
            
            var iterator = value.GetType()
                                .GetProperties()
                                .Where(x => x.CanRead)
                                .Where(x => x.PropertyType != typeof(string))
                                .Where(x => x.PropertyType.IsClass || x.PropertyType.IsInterface)
                                .Where(x => x.IsDefined(typeof(NullCheckAttribute), true))
                                .Where(x => x.GetCustomAttributes<NullCheckAttribute>().Any(a => a.Lifetime == UniTestLifetime.Constructor))
                                .Select(x => new PropertyNullable
                                {
                                    Result = x.GetValue(value) is not null,
                                    Name = x.Name,
                                    PropertyType = x.PropertyType
                                }).ToArray();
            
            Assert.IsTrue(iterator.All(x => x.Result));
        }
        
        public static void AssertAllFieldWasNotNull(object value)
        {
            if (value is null)
            {
                Assert.Fail(nameof(value));
            }
            
            var iterator = value.GetType()
                                .GetFields()
                                .Where(x => x.FieldType != typeof(string))
                                .Where(x => x.FieldType.IsClass || x.FieldType.IsInterface)
                                .Where(x => x.IsDefined(typeof(NullCheckAttribute), true))
                                .Where(x => x.GetCustomAttributes<NullCheckAttribute>().Any(a => a.Lifetime == UniTestLifetime.Constructor))
                                .Select(x => new PropertyNullable
                                {
                                    Result       = x.GetValue(value) is not null,
                                    Name         = x.Name,
                                    PropertyType = x.FieldType
                                });
            
            Assert.IsTrue(iterator.All(x => x.Result));
        }

        public static void AssertFieldEquals<T>(object instance, string fieldName, T source)
        {
            if (instance is null)
            {
                Assert.Fail(nameof(instance));
            }

            var field = instance.GetType()
                                .GetField(fieldName);
            
            Assert.IsNotNull(field);
            var fieldValue = field.GetValue(instance);
            
            Assert.IsInstanceOfType(fieldValue, typeof(T));
            Assert.AreEqual(fieldValue, source);
        }
        
        public static void AssertFieldIsNotNull(object instance, string fieldName)
        {
            if (instance is null)
            {
                Assert.Fail(nameof(instance));
            }

            var field = instance.GetType()
                                .GetField(fieldName);
            
            Assert.IsNotNull(field);
        }
        
        public static void AssertFieldIsNull(object instance, string fieldName)
        {
            if (instance is null)
            {
                Assert.Fail(nameof(instance));
            }

            var field = instance.GetType()
                                .GetField(fieldName);
            
            Assert.IsNull(field);
        }
    }
}