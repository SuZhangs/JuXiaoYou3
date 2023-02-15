namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class DegreeProperty : ModuleProperty, IOppositeSink
    {
        /// <summary>
        /// 负面值
        /// </summary>
        public string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        public string Positive { get; set; }
        
        /// <summary>
        /// 分界线
        /// </summary>
        public int DivideLine { get; set; }
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var value = Math.Clamp(Value.ToInt(1), 1, 10) > DivideLine ? Positive : Negative;
            return $"{Name}:\x20{value}";
        }


        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
}