using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Models
{
    public class ViewParam
    {
        /// <summary>
        /// 参数
        /// </summary>
        public object[] Parameters { get; init; }
        
        /// <summary>
        /// 来源视图模型
        /// </summary>
        internal IViewModel ViewModelSource { get; set; }

        /// <summary>
        /// 关闭对话框处理器
        /// </summary>
        internal Action CloseHandler { get; set; }
    }
}