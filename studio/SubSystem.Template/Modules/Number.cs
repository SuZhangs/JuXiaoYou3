using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public class NumberDataUI : ModuleBlockDataUI<NumberBlock, int>, INumberBlockDataUI
    {
        public NumberDataUI(NumberBlock block) : base(block)
        {
            Maximum = block.Maximum;
            Minimum = block.Minimum;
            Suffix  = block.Suffix;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; }
    }

    /// <summary>
    /// 表示数字内容块。
    /// </summary>
    public class NumberEditUI : ModuleBlockEditUI<NumberBlock, int>, INumberBlockEditUI
    {
        private string _suffix;
        private int    _maximum;
        private int    _minimum;

        public NumberEditUI(NumberBlock block) : base(block)
        {
            Suffix  = block.Suffix;
            Maximum = block.Maximum;
            Minimum = block.Minimum;
        }

        protected override NumberBlock CreateInstanceOverride()
        {
            return new NumberBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                Suffix   = Suffix,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
            };
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix
        {
            get => _suffix;
            set => SetValue(ref _suffix, value);
        }
    }
}