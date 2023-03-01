
namespace Acorisoft.FutureGL.Forest.Interfaces
{
    
    public interface ITabViewModel :IRootViewModel
    {
        /// <summary>
        /// 唯一标识符。
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 是否初始化。
        /// </summary>
        bool Initialized { get; }
        
        /// <summary>
        /// 标题。
        /// </summary>
        string Title { get; }
        
        /// <summary>
        /// 唯一性
        /// </summary>
        public bool Uniqueness { get; }
    }
}