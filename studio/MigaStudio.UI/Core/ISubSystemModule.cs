using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.AppModels;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface ISubSystemModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ITabViewController GetController();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="container"></param>
        void Initialize(CultureArea culture, IContainer container);

        /// <summary>
        /// 预览
        /// </summary>
        CultureArea Language { get; }


        /// <summary>
        /// 当前名字
        /// </summary>
        string FriendlyName { get; }
        string ModuleId { get; }
    }

    public abstract class SubSystemModule : ISubSystemModule
    {
        public abstract ITabViewController GetController();

        public void Initialize(CultureArea culture, IContainer container)
        {
            //
            // 设置语言
            Language     = culture;
            
            //
            // 设置名字
            FriendlyName = GetSubSystemName(culture);
            
            //
            // 注册语言
            var textSource = InstallLanguages(culture);

            //
            //
            Forest.Services
                  .Language
                  .AppendLanguageSource(textSource);

            //
            // 注册视图
            InstallView(container);

            //
            // 注册服务
            InstallServices(container);
        }

        protected abstract string GetSubSystemName(CultureArea language);

        protected abstract IEnumerable<string> InstallLanguages(CultureArea culture);
        protected abstract void InstallView(IContainer container);
        protected abstract void InstallServices(IContainer container);
        public CultureArea Language { get; private set; }
        public string FriendlyName { get; private set; }
        public abstract string ModuleId { get; }
    }
}