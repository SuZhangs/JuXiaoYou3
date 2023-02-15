namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    /// <summary>
    /// <see cref="TextPropertyEditView"/> 类型表示文本属性的编辑视图，
    /// </summary>
    public class TextPropertyEditView : PropertyEditView<TextProperty, string>, IBindableSuffixSink
    {
        private string _suffix;
        
        public TextPropertyEditView(TextProperty property) : base(property)
        {
            Suffix = property.Suffix;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ConstructFallback(string value) => value;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string DeconstructFallback(string value)=> value;

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override TextProperty FinishState()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            TargetProperty.Suffix   = Suffix;
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) ;
        
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
    }
    
    /// <summary>
    /// <see cref="TextPropertyDataView"/> 类型表示文本属性的数据视图，
    /// </summary>
    public class TextPropertyDataView : PropertyDataView<TextProperty, string>, ISuffixSink
    {
        public TextPropertyDataView(TextProperty property) : base(property)
        {
            Suffix = property.Suffix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GenericValueToString(string value) => value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string StringToGenericValue(string value)=> value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        protected override string OnValueChanged(string oldValue, string newValue, out bool fallback)
        {
            fallback = string.IsNullOrEmpty(newValue);
            return fallback ? TargetProperty.Fallback : newValue;
        }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; }
    }
}