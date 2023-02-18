using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlock
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 喵喵咒语
        /// </summary>
        string Metadata { get; }
        
        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        string ToolTips { get; }
    }
    
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlock<T>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        T Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        T Value { get; }
    }
    
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockEditUI
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// 喵喵咒语
        /// </summary>
        string Metadata { get; set; }
        
        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        string ToolTips { get; set; }
    }
    
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockEditUI<T>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        T Fallback { get; }
    }

    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public interface IModuleBlockDataUI : IModuleBlock
    {
    }
    
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlock : StorageObject
    {
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public abstract void ClearValue();
        
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 喵喵咒语
        /// </summary>
        public string Metadata { get; init; }
        
        /// <summary>
        /// 提示，可以是Markdown
        /// </summary>
        public string ToolTips { get; set; }
    }
}