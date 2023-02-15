namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class DegreePropertyDataView : PropertyDataView<DegreeProperty, int>
    {
        private string _display;
        
        public DegreePropertyDataView(DegreeProperty property) : base(property)
        {
            //
            // 请检查是否有Value的显式赋值
            // 请检查是否有需要绑定属性的赋值。
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            Display = Value >= TargetProperty.DivideLine ? TargetProperty.Positive : TargetProperty.Negative;
        }

        protected override string GenericValueToString(int value) => value.ToString();

        protected override int StringToGenericValue(string value) => value.ToInt(0);

        protected override int OnValueChanged(int oldValue, int newValue, out bool fallback)
        {
            UpdateDisplay();
            var val2 = Math.Clamp(newValue, 0, 10);
            fallback = val2 != newValue;
            return val2;
        }

        /// <summary>
        /// 获取或设置 <see cref="Display"/> 属性。
        /// </summary>
        public string Display
        {
            get => _display;
            set => SetValue(ref _display, value);
        }
    }
    
    public class DegreePropertyEditView : PropertyEditView<DegreeProperty, int>, IOppositeSink
    {
        private string _negative;
        private string _positive;
        private int    _divideLine;
        
        public DegreePropertyEditView(DegreeProperty property) : base(property)
        {
            Negative   = property.Negative;
            DivideLine = property.DivideLine;
            Positive   = property.Positive;
        }

        protected override string ConstructFallback(int value) => value.ToString();
        
        protected override int DeconstructFallback(string value) => value.ToInt(0);

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override DegreeProperty FinishState()
        {
            TargetProperty.Name       = Name;
            TargetProperty.Metadata   = Metadata;
            TargetProperty.Fallback   = ConstructFallback(Fallback);
            TargetProperty.Negative   = Negative;
            TargetProperty.Positive   = Positive;
            TargetProperty.DivideLine = DivideLine;
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected sealed override bool IsCompleted() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Negative) && !string.IsNullOrEmpty(Positive);
        
        /// <summary>
        /// 负面值
        /// </summary>
        public string Negative
        {
            get => _negative;
            set => SetValue(ref _negative, value);
        }

        /// <summary>
        /// 正面值
        /// </summary>
        public string Positive
        {
            get => _positive;
            set => SetValue(ref _positive, value);
        }

        /// <summary>
        /// 分界线
        /// </summary>
        public int DivideLine
        {
            get => _divideLine;
            set => SetValue(ref _divideLine, value);
        }
    }
}