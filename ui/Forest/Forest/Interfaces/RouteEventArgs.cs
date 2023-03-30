﻿namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// 参数
    /// </summary>
    public class RouteEventArgs : Parameter
    {
        
        /// <summary>
        /// 来源视图模型，在DialogViewModel中使用！
        /// </summary>
        internal IViewModel ViewModelSource { get; set; }

        /// <summary>
        /// 关闭对话框处理器，在DialogViewModel中使用！
        /// </summary>
        internal Action CloseHandler { get; set; }
    }

    public class Parameter
    {
        /// <summary>
        /// 参数
        /// </summary>
        public object[] Args { get; init; }
    }
}