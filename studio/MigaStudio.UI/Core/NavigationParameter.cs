using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class NavigationParameter
    {
        private NavigationParameter(Parameter args)
        {
            Params = args;
        }

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

        public static NavigationParameter FromParameter(Parameter parameter)
        {
            return new NavigationParameter(parameter);
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id => Document.Id ?? Index.Id;

        /// <summary>
        /// 目标文档
        /// </summary>
        public IData Document
        {
            get => (IData)Params.Args[1];
        }

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
        public Parameter Params { get; }

        /// <summary>
        /// 
        /// </summary>
        public ITabViewController Controller
        {
            get => (ITabViewController)Params.Args[0];
        }
    }
}