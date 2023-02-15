namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class ColorProperty : ModuleProperty
    {
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            return $"{Name}:\x20{Value}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown()
        {
            return $"{Name}: <code style=\"color:{Value};\">Sample</code>\x20\x20";
        }
    }
}