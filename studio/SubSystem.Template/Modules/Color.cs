using System.Windows.Media;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockDataUI : ModuleBlockDataUI, IColorBlockDataUI
    {
        private Color  _value;
        private string _hex;
        
        public ColorBlockDataUI(ColorBlock block) : base(block)
        {
            TargetBlock = block;
            Fallback    = Xaml.FromHex(block.Fallback);
            Value       = string.IsNullOrEmpty(block.Value) ? Fallback : Xaml.FromHex(block.Value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected ColorBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public Color Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public Color Value
        {
            get => _value;
            set
            {
                if (SetValue(ref _value, value))
                {
                    Hex               = value.ToString();
                    TargetBlock.Value = Hex;
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Hex"/> 属性。
        /// </summary>
        public string Hex
        {
            get => _hex;
            set => SetValue(ref _hex, value);
        }
    }
    
    /// <summary>
    /// 表示颜色内容块。
    /// </summary>
    public class ColorBlockEditUI : ModuleBlockEditUI<ColorBlock, string>, IColorBlockEditUI
    {
        public ColorBlockEditUI(ColorBlock block) : base(block)
        {
        }

        protected override ColorBlock CreateInstanceOverride()
        {
            return new ColorBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
                Fallback = Fallback,
            };
        }
    }
}