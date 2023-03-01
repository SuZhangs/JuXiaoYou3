using System.Diagnostics;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class NavigationParameter
    {
        private NavigationParameter(Parameter args)
        {
            Params = args;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter Test()
        {
            var args = new Parameter
            {
                Args = new object[8]
            };

            args.Args[0] = ID.Get();
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cache">索引</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter New(IData data, IDataCache cache, ITabViewController controller)
        {
            var args = new Parameter
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = data;
            args.Args[2] = cache;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter New(ITabViewModel viewModel, ITabViewController controller)
        {
            var args = new Parameter
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = viewModel.Uniqueness ? 
                viewModel.GetType().FullName : 
                    string.IsNullOrEmpty(viewModel.Id) ? 
                    Guid.NewGuid().ToString("N"): viewModel.Id;
            return new NavigationParameter(args);
        }

        /// <summary>
        /// 从一个常规的视图参数中创建
        /// </summary>
        /// <param name="parameter">视图参数</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter FromParameter(Parameter parameter)
        {
            return new NavigationParameter(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        public ITabViewController Controller
        {
            get => (ITabViewController)Params.Args[0];
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id => (string)Params.Args[1];

        /// <summary>
        /// 目标文档
        /// </summary>
        public IData Document
        {
            get => (IData)Params.Args[2];
        }

        /// <summary>
        /// 目标索引
        /// </summary>
        public IDataCache Index
        {
            get => (IDataCache)Params.Args[3];
        }

        /// <summary>
        /// 参数
        /// </summary>
        public Parameter Params { get; }
    }
}