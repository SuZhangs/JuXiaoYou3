namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{

    public class NumberProperty : ModuleProperty, ISuffixSink, IClampSink
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; set; }
        
        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; set; }
        
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
            var value = Math.Clamp(Value.ToInt(Minimum), Minimum, Maximum);
            return $"{Name}:\x20{value}{Suffix}";
        }


        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
}