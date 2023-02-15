using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class HistogramPropertyDataView : PropertyDataView<HistogramProperty, int[]>
    {
        public HistogramPropertyDataView(HistogramProperty property) : base(property)
        {
            Axis = TargetProperty.Axis;
        }

        protected override string GenericValueToString(int[] value) => string.Join(',', value.Select(x => x.ToString()));

        protected override int[] StringToGenericValue(string value) => value.Split(',')
            .Select(x => x.ToInt(0))
            .ToArray();

        protected override int[] OnValueChanged(int[] oldValue, int[] newValue, out bool fallback)
        {
            fallback = oldValue.Length != newValue.Length ||
                       !oldValue.SequenceEqual(newValue);

            return fallback ? StringToGenericValue(TargetProperty.Fallback) : newValue;
        }
        
        public string[] Axis { get; }
    }

    public class HistogramPropertyEditView : PropertyEditView<HistogramProperty, int[]>, IClampSink
    {
        private int   _maximum;
        private Color _color;

        public HistogramPropertyEditView(HistogramProperty property) : base(property)
        {
            Axis = new ObservableCollection<string>();
            if (property.Axis is not null)
            {
                Axis.AddRange(property.Axis);
            }
        }
        
        public ObservableCollection<string> Axis { get; }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }
        
        public int Minimum
        {
            get => 0;
        }


        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected sealed override bool IsCompleted() => !string.IsNullOrEmpty(Name);

        protected override string ConstructFallback(int[] value)=> string.Join(',', value.Select(x => x.ToString()));

        protected override int[] DeconstructFallback(string value) => value.Split(',')
            .Select(x => x.ToInt(0))
            .ToArray();

        protected override HistogramProperty FinishState()
        {
            TargetProperty.FallbackValues = Fallback;
            TargetProperty.Maximum        = Maximum;
            TargetProperty.Color          = Color.ToString();
            TargetProperty.Axis           = Axis.ToArray();
            TargetProperty.Name           = Name;
            TargetProperty.Metadata       = Metadata;
            return TargetProperty;
        }
    }

    public class RadarPropertyDataView : PropertyDataView<RadarProperty, int[]>
    {
        public RadarPropertyDataView(RadarProperty property) : base(property)
        {
            Axis = TargetProperty.Axis;
        }

        protected override string GenericValueToString(int[] value) => string.Join(',', value.Select(x => x.ToString()));

        protected override int[] StringToGenericValue(string value) => value.Split(',')
            .Select(x => x.ToInt(0))
            .ToArray();

        protected override int[] OnValueChanged(int[] oldValue, int[] newValue, out bool fallback)
        {
            fallback = oldValue.Length != newValue.Length ||
                       !oldValue.SequenceEqual(newValue);

            return fallback ? StringToGenericValue(TargetProperty.Fallback) : newValue;
        }
        
        public string[] Axis { get; }
    }

    public class RadarPropertyEditView : PropertyEditView<RadarProperty, int[]>, IClampSink
    {
        private int   _maximum;
        private Color _color;

        public RadarPropertyEditView(RadarProperty property) : base(property)
        {
            Axis = new ObservableCollection<string>();
            if (property.Axis is not null)
            {
                Axis.AddRange(property.Axis);
            }
        }
        
        public ObservableCollection<string> Axis { get; }

        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }
        
        public int Minimum
        {
            get => 0;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected sealed override bool IsCompleted() => !string.IsNullOrEmpty(Name);

        protected override string ConstructFallback(int[] value)=> string.Join(',', value.Select(x => x.ToString()));

        protected override int[] DeconstructFallback(string value) => value.Split(',')
            .Select(x => x.ToInt(0))
            .ToArray();

        protected override RadarProperty FinishState()
        {
            TargetProperty.FallbackValues = Fallback;
            TargetProperty.Maximum        = Maximum;
            TargetProperty.Color          = Color.ToString();
            TargetProperty.Axis           = Axis.ToArray();
            TargetProperty.Name           = Name;
            TargetProperty.Metadata       = Metadata;
            return TargetProperty;
        }
    }
}