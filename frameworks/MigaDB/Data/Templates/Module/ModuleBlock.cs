using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示一个模组内容块。
    /// </summary>
    public abstract class ModuleBlock : StorageObject
    {
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