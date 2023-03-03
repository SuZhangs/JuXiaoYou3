using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using Acorisoft.FutureGL.MigaUtils;
using DynamicData;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class ChartBlockDataUI :  ModuleBlockDataUI, IChartBlockDataUI
    {
        private List<BindableAxis> _value;

        protected ChartBlockDataUI(ChartBlock block) : base(block)
        {
            TargetBlock = block;
            Fallback    = block.Fallback;
            Value       = new List<BindableAxis>();
            Axis        = new List<string>(block.Axis);
            Color       = block.Color;
            Maximum     = block.Maximum;
            Minimum     = block.Minimum;
            var value = block.Value is null || block.Value.Length == 0 ? block.Fallback : block.Value;

            for (var i = 0; i < Value.Count; i++)
            {
                Value.Add(new BindableAxis(OnValueChanged, Axis[i], i, value[i], Maximum));
            }
        }
        
        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        protected void OnValueChanged(int index, int value)
        {
            TargetBlock.Value[index] = value;
        }
        
        protected ChartBlock TargetBlock { get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public int[] Fallback { get; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public List<BindableAxis> Value
        {
            get => _value;
            set
            {
                
            }
        }


        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; }

        public List<string> Axis { get; }
        
        public string Color { get; }
    }

    public class BindableAxis : ObservableObject
    {
        public BindableAxis(Action<int, int> callback, string name, int index, int value, int max)
        {
            Callback = callback;
            Index    = index;
            Name     = name;
            Maximum  = max;
            _value   = value;
        }

        private int    _value;
        private string _name;
        
        public Action<int, int> Callback { get; }
        
        public int Maximum { get; }
        public int Index { get; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                SetValue(ref _value, value);
                Callback?.Invoke(Index, _value);
            }
        }
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