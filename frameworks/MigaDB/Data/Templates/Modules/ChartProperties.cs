using System.Text;
using Acorisoft.FutureGL.MigaDB.Contracts;
using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public abstract class ChartProperty : ModuleProperty, IClampSink
    {
        private const char Unselected = '━';
        private const char Selected   = '■';

        public sealed override void ClearValue()
        {
            ChartValues = null;
            base.ClearValue();
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        
        /// <summary>
        /// 轴定义
        /// </summary>
        public string[] Axis { get; set; }

        /// <summary>
        /// 默认值。
        /// </summary>
        public int[] FallbackValues
        {
            get => (string.IsNullOrEmpty(Fallback) ? "1" : Fallback)
                .Split(',')
                .Select(x => int.TryParse(x, out var n) ? n : 1)
                .ToArray();
            set
            {
                Fallback = string.Join(',', (value ?? Array.Empty<int>()).Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// 默认值。
        /// </summary>
        public int[] ChartValues
        {
            get => (string.IsNullOrEmpty(Value) ? "1" : Value)
                .Split(',')
                .Select(x => int.TryParse(x, out var n) ? n : 1)
                .ToArray();
            set
            {
                Value = string.Join(',', (value ?? Array.Empty<int>()).Select(x => x.ToString()));
            }
        }
        
        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; set; }
        
        /// <summary>
        /// 最大值
        /// </summary>
        public int Minimum { get; set; }
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public string GetLine(int value)
        {
            var time = (Maximum / 10);
            var roundedValue = Math.Clamp(value, Minimum, Maximum) / time;
            
            var array = new char[10];
            Array.Fill(array, Unselected);
            array[roundedValue] = Selected;
            return new string(array);
        }
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var maxAxisText = Axis.Max(x => x.Length) + 4;
            var sb = new StringBuilder();
            var minLength = Math.Min(FallbackValues.Length, ChartValues.Length);
            sb.Append(Name);
            sb.Append(Environment.NewLine);
            
            // Asix1:   Value

            for (var i = 0; i < minLength; i++)
            {
                var axis = Axis[i];
                var value = GetLine(ChartValues[i]);
                var whitespace = "\x20".Repeat(maxAxisText - axis.Length);
                sb.Append($"{axis}:{whitespace}{value}{Environment.NewLine}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown()
        {
            return $"{ToPlainText()}\x20\x20";
        }
    }
    
    public class HistogramProperty : ChartProperty
    {
    }

    public class RadarProperty : ChartProperty
    {
    }
}