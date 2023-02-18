using System;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using DynamicData;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class ChartBlockDataUI : ModuleBlockDataUI<ChartBlock, int[]>, IChartBlockDataUI
    {
        protected ChartBlockDataUI(ChartBlock block) : base(block)
        {
            Axis    = block.Axis;
            Color   = block.Color;
            Maximum = block.Maximum;
            Minimum = block.Minimum;
        }

        protected override int[] OnValueChanged(int[] oldValue, int[] newValue)
        {
            if (newValue is null || newValue.Length == 0)
            {
                newValue = Fallback;
            }

            for (var i = 0; i < newValue.Length; i++)
            {
                newValue[i] = Math.Clamp(newValue[i], Minimum, Maximum);
            }

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

        public string[] Axis { get; }
        public string Color { get; }
    }

    public abstract class ChartBlockEditUI : ModuleBlockEditUI<ChartBlock, int[]>, IChartBlockEditUI
    {
        private int    _maximum;
        private int    _minimum;
        private string _color;

        protected ChartBlockEditUI(ChartBlock block) : base(block)
        {
            Axis    = new ObservableCollection<string>();
            Maximum = block.Maximum;
            Minimum = block.Minimum;
            Color   = block.Color;
            
            if (block.Axis is not null)
            {
                Axis.AddRange(block.Axis);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetValue(ref _color, value);
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

        public ObservableCollection<string> Axis { get; }
    }

    public class RadarBlockDataUI : ChartBlockDataUI
    {
        public RadarBlockDataUI(RadarBlock block) : base(block)
        {
        }
    }
    
    public class HistogramBlockDataUI : ChartBlockDataUI
    {
        public HistogramBlockDataUI(HistogramBlock block) : base(block)
        {
        }
    }
    
    public class RadarBlockEditUI : ChartBlockEditUI
    {
        public RadarBlockEditUI(RadarBlock block) : base(block)
        {
        }

        protected override ChartBlock CreateInstanceOverride()
        {
            return new RadarBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
                Axis     = Axis.ToArray(),
                Color    = Color,
            };
        }
    }
    
    public class HistogramBlockEditUI : ChartBlockEditUI
    {
        public HistogramBlockEditUI(HistogramBlock block) : base(block)
        {
        }
        
        protected override ChartBlock CreateInstanceOverride()
        {
            return new HistogramBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
                Maximum  = Maximum,
                Minimum  = Minimum,
                Axis     = Axis.ToArray(),
                Color    = Color,
            };
        }
    }
}