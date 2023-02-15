using System.Runtime.CompilerServices;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class SliderPropertyDataView : PropertyDataView<SliderProperty, int>, IBindableClampSink
    {
        public SliderPropertyDataView(SliderProperty property) : base(property)
        {
            Maximum = property.Maximum;
            Minimum = property.Minimum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GenericValueToString(int value) => value.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int StringToGenericValue(string value) => Math.Clamp(value.ToInt(Minimum), Minimum, Maximum);

        protected override int OnValueChanged(int oldValue, int newValue, out bool fallback)
        {
            var val2 = Math.Clamp(newValue, Minimum, Maximum);
            fallback = val2 != newValue;
            return val2;
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

    public class SliderPropertyEditView : PropertyEditView<SliderProperty, int>, IClampSink
    {
        private int _maximum;
        private int _minimum;
        
        public SliderPropertyEditView(SliderProperty property) : base(property)
        {
            Maximum = property.Maximum;
            Minimum = property.Minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ConstructFallback(int value) => Math.Clamp(value, Minimum, Maximum).ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int DeconstructFallback(string value) => Math.Clamp(value.ToInt(Minimum), Minimum, Maximum);

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override SliderProperty FinishState()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            TargetProperty.Minimum  = Minimum;
            TargetProperty.Maximum  = Maximum;
            
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) && Fallback <= Maximum && Fallback >= Minimum;

        /// <summary>
        /// 获取或设置 <see cref="Minimum"/> 属性。
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }
    }
}