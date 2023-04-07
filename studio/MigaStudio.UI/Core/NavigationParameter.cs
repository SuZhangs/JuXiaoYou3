using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Parameter = System.Reflection.Metadata.Parameter;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class NavigationParameter
    {
        private NavigationParameter(RoutingEventArgs args)
        {
            Params = args;
        }
        
        //
        // 注意：
        // [0] Controller
        // [1] Id
        // [2] Target
        // [3] Parameter

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter Empty()
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = ID.Get();
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter NewPage(string id, ITabViewController controller)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = id;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cache">索引</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter OpenDocument(IDataCache cache, ITabViewController controller)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = cache.Id;
            args.Args[2] = cache;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="fileName">数据</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter OpenFile(string fileName, ITabViewController controller)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = fileName;
            args.Args[2] = fileName;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="fileName">数据</param>
        /// <param name="parameter">索引</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter OpenFile(string fileName, Parameter parameter, ITabViewController controller)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = fileName;
            args.Args[2] = fileName;
            args.Args[3] = parameter;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter NewPage(ITabViewModel viewModel, ITabViewController controller)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[8]
            };

            args.Args[0] = controller;
            args.Args[1] = viewModel.Uniqueness ? 
                viewModel.GetType().FullName : 
                    string.IsNullOrEmpty(viewModel.PageId) ? 
                    Guid.NewGuid().ToString("N"): viewModel.PageId;
            return new NavigationParameter(args);
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        /// <param name="controller">控制器</param>
        /// <param name="parameters">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter NewPage(ITabViewModel viewModel, ITabViewController controller, object[] parameters)
        {
            var args = new RoutingEventArgs
            {
                Args = new object[2 + (parameters?.Length ?? 0)]
            };

            args.Args[0] = controller;
            args.Args[1] = viewModel.Uniqueness ? 
                viewModel.GetType().FullName : 
                string.IsNullOrEmpty(viewModel.PageId) ? 
                    Guid.NewGuid().ToString("N"): viewModel.PageId;
            return new NavigationParameter(args);
        }

        /// <summary>
        /// 从一个常规的视图参数中创建
        /// </summary>
        /// <param name="parameter">视图参数</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static NavigationParameter FromParameter(RoutingEventArgs parameter)
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
        /// 目标索引
        /// </summary>
        public IDataCache Index
        {
            get => (IDataCache)Params.Args[2];
        }

        /// <summary>
        /// 参数
        /// </summary>
        public RoutingEventArgs Params { get; }
    }
}