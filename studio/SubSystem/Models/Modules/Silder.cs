using System;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules
{
    public class SliderBlockDataUI : ModuleBlockDataUI<SliderBlock, int>, INumberBlockDataUI
    {
        public SliderBlockDataUI(SliderBlock block) : base(block)
        {
            Maximum = block.Maximum;
            Minimum = block.Minimum;
            Suffix  = block.Suffix;
            Value = block.Value;
        }

        protected override int OnValueChanged(int oldValue, int newValue)
        {
            newValue          = newValue == -1 ? Fallback : Math.Clamp(newValue, Minimum, Maximum);
            TargetBlock.Value = newValue;
            return newValue;
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


    public class SliderBlockEditUI  : ModuleBlockEditUI<SliderBlock, int>, INumberBlockEditUI
    {
        private string _suffix;
        private int    _maximum;
        private int    _minimum;

        public SliderBlockEditUI(SliderBlock block) : base(block)
        {
            Suffix  = block.Suffix;
            Maximum = block.Maximum;
            Minimum = block.Minimum;
        }

        protected override SliderBlock CreateInstanceOverride()
        {
            return new SliderBlock
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