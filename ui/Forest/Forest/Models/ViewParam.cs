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
        public IViewModel ViewModelSource { get; init; }
    }
}