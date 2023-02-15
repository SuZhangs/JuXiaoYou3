
namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class BinaryPropertyDataView : PropertyDataView<BinaryProperty, bool>
    {
        public BinaryPropertyDataView(BinaryProperty property) : base(property)
        {
            // 
            // 注意:
            // Negative 和 Positive属性不需要绑定吧
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            Display = Value ? TargetProperty.Positive : TargetProperty.Negative;
        }

        protected override string GenericValueToString(bool value) => value.ToString();

        protected override bool StringToGenericValue(string value) => value.ToBoolean(false);

        protected override bool OnValueChanged(bool oldValue, bool newValue, out bool fallback)
        {
            UpdateDisplay();
            fallback = false;
            return newValue;
        }

        private string _display;

        /// <summary>
        /// 获取或设置 <see cref="Display"/> 属性。
        /// </summary>
        public string Display
        {
            get => _display;
            set => SetValue(ref _display, value);
        }
    }
    
    public class BinaryPropertyEditView : PropertyEditView<BinaryProperty, bool>, IOppositeSink
    {
        private string _negative;
        private string _positive;
        
        public BinaryPropertyEditView(BinaryProperty property) : base(property)
        {
            Negative   = property.Negative;
            Positive   = property.Positive;
        }

        protected override string ConstructFallback(bool value) => value.ToString();
        
        protected override bool DeconstructFallback(string value) => value.ToBoolean(false);

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override BinaryProperty FinishState()
        {
            //
            // 请检查是否有Value的显式赋值
            // 请检查是否有需要绑定属性的赋值。
            TargetProperty.Name       = Name;
            TargetProperty.Metadata   = Metadata;
            TargetProperty.Fallback   = ConstructFallback(Fallback);
            TargetProperty.Negative   = Negative;
            TargetProperty.Positive   = Positive;
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
    }
}