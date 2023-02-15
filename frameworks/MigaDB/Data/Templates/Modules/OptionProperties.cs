using Acorisoft.FutureGL.MigaDB.Contracts;
using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class SwitchProperty : ModuleProperty
    {
        private const char Checked   = '✓';
        private const char Unchecked = '×';
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var flag = Value.ToBoolean(false) ? Checked : Unchecked;
            return $"{Name}:\x20{flag}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
    
    public class RadioProperty : ModuleProperty, IChildrenSink<RadioProperty.Item>
    {
        public class Item : ObservableObject
        {
            private string _name;
            private string _value;

            /// <summary>
            /// 获取或设置 <see cref="Value"/> 属性。
            /// </summary>
            public string Value
            {
                get => _value;
                set => SetValue(ref _value, value);
            }
            
            /// <summary>
            /// 获取或设置 <see cref="Name"/> 属性。
            /// </summary>
            public string Name
            {
                get => _name;
                set => SetValue(ref _name, value);
            }
        }

        public Item[] Items { get; set; }
        
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
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }

    public class TalentProperty : ModuleProperty
    {
        private const char Checked   = '★';
        private const char Unchecked = '☆';

        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var flag = Value.ToBoolean(false) ? Checked : Unchecked;
            return $"{Name}:\x20{flag}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }

    public class FavoriteProperty : ModuleProperty
    {
        private const char Checked = '♥';
        private const char Unchecked = '♡';
        
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        public sealed override string ToPlainText()
        {
            var flag = Value.ToBoolean(false) ? Checked : Unchecked;
            return $"{Name}:\x20{flag}";
        }

        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
    
    public class BinaryProperty : ModuleProperty, IOppositeSink
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
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }

    public class SequenceProperty : ModuleProperty, IChildrenSink<SequenceProperty.Item>
    {
        public class Item : ObservableObject
        {
            private string _name;
            
            /// <summary>
            /// 获取或设置 <see cref="Name"/> 属性。
            /// </summary>
            public string Name
            {
                get => _name;
                set => SetValue(ref _name, value);
            }
        }

        public Item[] Items { get; set; }
        
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
        public sealed override string ToMarkdown() => $"{ToPlainText()}\x20\x20";
    }
}