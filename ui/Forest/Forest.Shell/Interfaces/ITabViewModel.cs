﻿
namespace Acorisoft.FutureGL.Forest.Interfaces
{
    
    public interface ITabViewModel :IRootViewModel, IApprovalRequired
    {
        /// <summary>
        /// 唯一标识符。
        /// </summary>
        string PageId { get; }
        
        /// <summary>
        /// 是否初始化。
        /// </summary>
        bool Initialized { get; }
        
        /// <summary>
        /// 标题。
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// 唯一性
        /// </summary>
        bool Uniqueness { get; }
        
        /// <summary>
        /// 是否可被关闭
        /// </summary>
        bool Removable { get; }
    }
}