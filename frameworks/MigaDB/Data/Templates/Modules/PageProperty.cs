
namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class PageProperty : ModuleProperty
    {
        /// <summary>
        /// 是否开启高亮模式
        /// </summary>
        public bool EnableHighlight { get; set; }
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            return $"{Name}: {Environment.NewLine}{Value}";
        }
        
        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }



}