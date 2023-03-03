using System;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class CounterBlockDataUI : ModuleBlockDataUI<CounterBlock, int>, ICounterBlockDataUI
    {
        protected CounterBlockDataUI(CounterBlock block) : base(block)
        {
            Maximum = block.Maximum;
            Minimum = block.Minimum;
            
            if (block.Value == -1)
            {
                block.Value = block.Fallback;
            }

            Value = block.Value;
        }

        protected sealed override int OnValueChanged(int oldValue, int newValue)
        {
            newValue          = Math.Clamp(newValue, Minimum, Maximum);
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
    }

    public class RateBlockDataUI : CounterBlockDataUI
    {
        public RateBlockDataUI(RateBlock block) : base(block)
        {
        }
    }

    public class LikabilityBlockDataUI : CounterBlockDataUI
    {
        public LikabilityBlockDataUI(LikabilityBlock block) : base(block)
        {
        }
    }

    public abstract class CounterBlockEditUI : ModuleBlockEditUI<CounterBlock, int>, ICounterBlockEditUI
    {
        private int _maximum;
        private int _minimum;

        protected CounterBlockEditUI(CounterBlock block) : base(block)
        {
            Maximum = block.Maximum;
            Minimum = block.Minimum;
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
    }

    public class RateBlockEditUI : CounterBlockEditUI
    {
        public RateBlockEditUI(RateBlock block) : base(block)
        {
        }

        protected override CounterBlock CreateInstanceOverride()
        {
            return new RateBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
            };
        }
    }

    public class LikabilityBlockEditUI : CounterBlockEditUI
    {
        public LikabilityBlockEditUI(LikabilityBlock block) : base(block)
        {
        }

        protected override CounterBlock CreateInstanceOverride()
        {
            return new LikabilityBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
            };
        }
    }
}