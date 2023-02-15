using System.Text;
using Acorisoft.FutureGL.MigaDB.Contracts;
using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class GroupProperty : ModuleProperty, IChildrenSink<ModuleProperty>
    {
        public ModuleProperty[] Items { get; set; }

        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var sb = new StringBuilder($"{Name}:");
            foreach (var property in Items)
            {
                sb.Append(property.ToPlainText());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown()
        {
            var sb = new StringBuilder($"{Name}:");
            foreach (var property in Items)
            {
                sb.Append(property.ToMarkdown());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
    
    public class RateProperty : ModuleProperty, IClampSink
    {
        private const char Checked   = '★';
        private const char Unchecked = '☆';
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
            
            var array = new char[roundedValue];
            Array.Fill(array, Checked);
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
    
    public class LikabilityProperty : ModuleProperty, IClampSink
    {
        private const char Checked   = '♥';
        private const char Unchecked = '♡';
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
            
            var array = new char[roundedValue];
            Array.Fill(array, Checked);
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