
namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    
    public class PagePropertyDataView : PropertyDataView<PageProperty, string>
    {
        public PagePropertyDataView(PageProperty property) : base(property)
        {
            EnableHighlight = property.EnableHighlight;
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

        protected override string OnValueChanged(string oldValue, string newValue, out bool fallback)
        {
            fallback = string.IsNullOrEmpty(newValue);
            return fallback ? TargetProperty.Fallback : newValue;
        }
        
        public bool EnableHighlight { get; }
    }
    
    public class PagePropertyEditView : PropertyEditView<PageProperty, string>
    {
        private bool _enableHighlight;
        
        public PagePropertyEditView(PageProperty property) : base(property)
        {
            EnableHighlight = property.EnableHighlight;
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
        protected override PageProperty FinishState()
        {
            TargetProperty.Name            = Name;
            TargetProperty.Metadata        = Metadata;
            TargetProperty.Fallback        = ConstructFallback(Fallback);
            TargetProperty.EnableHighlight = EnableHighlight;
            return TargetProperty;
        }
        
        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) ;

        /// <summary>
        /// 获取或设置 <see cref="EnableHighlight"/> 属性。
        /// </summary>
        public bool EnableHighlight
        {
            get => _enableHighlight;
            set => SetValue(ref _enableHighlight, value);
        }
    }
}