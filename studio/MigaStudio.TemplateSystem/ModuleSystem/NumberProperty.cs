
namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class NumberPropertyDataView : PropertyDataView<NumberProperty, int>, IBindableClampSink, IBindableSuffixSink
    {
        public NumberPropertyDataView(NumberProperty property) : base(property)
        {
            Maximum = property.Maximum;
            Minimum = property.Minimum;
            Suffix  = property.Suffix;
        }
        
        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GenericValueToString(int value) => value.ToString();

        /// <summary>
        /// 转成整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; }
    }
    
    public class NumberPropertyEditView : PropertyEditView<NumberProperty, int>, IClampSink, ISuffixSink
    {
        private int    _maximum;
        private int    _minimum;
        private string _suffix;
        
        public NumberPropertyEditView(NumberProperty property) : base(property)
        {
            Maximum = property.Maximum;
            Minimum = property.Minimum;
            Suffix  = property.Suffix;
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
        protected override NumberProperty FinishState()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            TargetProperty.Minimum  = Minimum;
            TargetProperty.Maximum  = Maximum;
            TargetProperty.Suffix   = Suffix;
            
            return TargetProperty;
        }


        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) && Fallback <= Maximum && Fallback >= Minimum;
        
        /// <summary>
        /// 获取或设置 <see cref="Suffix"/> 属性。
        /// </summary>
        public string Suffix
        {
            get => _suffix;
            set
            {
                SetValue(ref _suffix, value);
                TargetProperty.Suffix = value;
            }
        }
        
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