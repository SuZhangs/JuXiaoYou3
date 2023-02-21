using System.IO;
using System.Reactive.Concurrency;
using System.Threading;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.Views;
using DryIoc;

namespace Acorisoft.FutureGL.Forest
{
    public abstract class ForestApp : Application
    {
        internal class BuiltinViews : IBindingInfoProvider
        {
            public IEnumerable<BindingInfo> GetBindingInfo()
            {
                return new[]
                {
                    new BindingInfo
                    {
                        ViewModel = typeof(DangerViewModel),
                        View      = typeof(DangerView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(WarningViewModel),
                        View      = typeof(WarningView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(SuccessViewModel),
                        View      = typeof(SuccessView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(InfoViewModel),
                        View      = typeof(InfoView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(ObsoleteViewModel),
                        View      = typeof(ObsoleteView)
                    },
                    new BindingInfo
                    {
                        ViewModel = typeof(StringViewModel),
                        View      = typeof(StringView)
                    },
                };
            }
        }

        protected const string BasicSettingFileName = "main.json";

        /*
         * 生命周期:
         *          sCtor
         *            |
         *          Ctor
         *            |
         *     RegisterServices (Internal)
         *            |
         *     RegisterServices (Public)
         *            |
         *       RegisterViews
         *            |
         *       ViewModel.Start
         *            |
         *  RegisterDependentServices
         *            |
         *        SplashView
         *            |
         *        
         */
        protected ForestApp()
        {
            //
            // 初始化
            Initialize();
        }

        private void Initialize()
        {
            RegisterFrameworkServices(Xaml.Container);
            RegisterServices(Xaml.Container);
            RegisterViews(Xaml.Container);
        }

        protected virtual string GetLanguageFile()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English => "en.ini",
                CultureArea.French => "fr.ini",
                CultureArea.Japanese => "jp.ini",
                CultureArea.Korean => "kr.ini",
                CultureArea.Russian => "ru.ini",
                _ => "cn.ini",
            };
            
            return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages"), fileName);
        }

        /// <summary>
        /// 注册框架服务。
        /// </summary>
        /// <param name="container">服务容器。</param>
        protected virtual void RegisterFrameworkServices(IContainer container)
        {
            var appModel = ConfigureDirectory();

            //
            // 构建基本的属性
            var basicAppSetting = JSON.OpenSetting<BasicAppSetting>(
                Path.Combine(appModel.Settings, BasicSettingFileName),
                () => new BasicAppSetting
                {
                    Language = CultureArea.Chinese,
                    Theme    = MainTheme.Light
                });

            //
            // 设置主题。
            ThemeSystem.Instance.Theme = GetThemeSystem(basicAppSetting);

            //
            // 设置语言。
            Language.Culture = basicAppSetting.Language;
            Language.SetLanguage(GetLanguageFile());

            //
            // 注册服务
            container.Use<BasicAppSetting>(basicAppSetting);
            container.Use<ApplicationModel>(appModel);
            container.Use<ForestResourceFactory, ITextResourceFactory>(new ForestResourceFactory());
            container.Use<WindowEventBroadcast, IWindowEventBroadcast, IWindowEventBroadcastAmbient>(new WindowEventBroadcast());
            container.Use<DialogService,
                IDialogService,
                IDialogServiceAmbient,
                IBusyService,
                IBusyServiceAmbient,
                INotifyServiceAmbient,
                INotifyService>(new DialogService());
            Xaml.InstallViewManually(new BuiltinViews());
        }

        protected virtual ApplicationModel ConfigureDirectory()
        {
            var domain = ApplicationModel.CheckDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Forest"));

            return new ApplicationModel
            {
                Logs      = Path.Combine(domain, "Logs"),
                Settings  = Path.Combine(domain, "UserData"),
                Languages = Path.Combine(domain, "Languages")
            }.Initialize();
        }

        /// <summary>
        /// 注册服务。
        /// </summary>
        /// <param name="container">服务容器。</param>
        protected abstract void RegisterServices(IContainer container);

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="container">服务容器。</param>
        protected abstract void RegisterViews(IContainer container);

        /// <summary>
        /// 注册上下文依赖的服务。
        /// </summary>
        /// <param name="container">服务容器。</param>
        protected internal virtual void RegisterContextServices(IContainer container)
        {
            container.Use<SynchronizationContextScheduler, IScheduler>(
                new SynchronizationContextScheduler(
                    SynchronizationContext.Current!));
        }

        /// <summary>
        /// 获得主题
        /// </summary>
        /// <returns>返回主题。</returns>
        /// <remarks>必须在启动前完成。</remarks>
        protected virtual ForestThemeSystem GetThemeSystem(BasicAppSetting basicAppSetting)
        {
            return basicAppSetting.Theme == MainTheme.Light ? new ForestLightTheme() : new ForestDarkTheme();
        }
    }
}