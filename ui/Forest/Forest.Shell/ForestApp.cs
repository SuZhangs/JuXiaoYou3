﻿using System.IO;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Utils;
using DryIoc;

namespace Acorisoft.FutureGL.Forest
{
    public abstract class ForestApp : Application
    {
        protected const string BasicSettingFileName = "theme.";

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

        protected virtual string GetLanguageFile(ApplicationModel appModel)
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
            
            return Path.Combine(appModel.Languages, fileName);
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
            var basicAppSetting = JSON.OpenSetting<BasicAppSettingViewModel>(
                Path.Combine(appModel.Settings, "main.json"),
                () => new BasicAppSettingViewModel
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
            Language.SetLanguage(GetLanguageFile(appModel));

            //
            // 注册服务
            container.Use<BasicAppSettingViewModel>(basicAppSetting);
            container.Use<ApplicationModel>(appModel);
            container.Use<ForestResolver, ILanguageNodeResolver>(new ForestResolver());
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
        protected virtual void RegisterContextServices(IContainer container)
        {
        }

        /// <summary>
        /// 获得主题
        /// </summary>
        /// <returns>返回主题。</returns>
        /// <remarks>必须在启动前完成。</remarks>
        protected virtual ForestThemeSystem GetThemeSystem(BasicAppSettingViewModel basicAppSetting)
        {
            return basicAppSetting.Theme == MainTheme.Light ? new ForestLightTheme() : new ForestDarkTheme();
        }
    }
}