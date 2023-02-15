using System.Runtime.CompilerServices;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class SliderProperty : ModuleProperty, IClampSink
    {
        private const char Unselected = '━';
        private const char Selected   = '■';

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; set; }


        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var fallback = Fallback.ToInt(1);
            var value = Value.ToInt(fallback);
            var time = (Maximum / 10);
            var roundedValue = value / time;

            var array = new char[10];
            Array.Fill(array, Unselected);
            array[roundedValue] = Selected;
            var val = new string(array);
            return $"{Name}:\x20{val}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
}