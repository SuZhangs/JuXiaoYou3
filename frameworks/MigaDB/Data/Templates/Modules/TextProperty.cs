namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{

    public class TextProperty : ModuleProperty, ISuffixSink
    {
        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            return $"{Name}:\x20{Value}{Suffix}";
        }


        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
}