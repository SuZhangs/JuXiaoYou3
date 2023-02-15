
using System.Windows.Media;
using Acorisoft.FutureGL.MigaUI;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    /// <summary>
    /// <see cref="ColorPropertyDataView"/> 类型表示颜色属性的数据视图，
    /// </summary>
    public class ColorPropertyDataView : PropertyDataView<ColorProperty, Color>
    {
        public ColorPropertyDataView(ColorProperty property) : base(property)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GenericValueToString(Color value) => value.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Color StringToGenericValue(string value)=> Xaml.FromHex(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        protected override Color OnValueChanged(Color oldValue, Color newValue, out bool fallback)
        {
            fallback = newValue.A == 0;
            return fallback ? Xaml.FromHex(TargetProperty.Fallback) : newValue;
        }
    }
    
    
    public class ColorPropertyEditView : PropertyEditView<ColorProperty, Color>
    {
        public ColorPropertyEditView(ColorProperty property) : base(property)
        {
            //
            // 请检查是否有Value的显式赋值
            // 请检查是否有需要绑定属性的赋值。
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ConstructFallback(Color value) => value.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Color DeconstructFallback(string value)=> Xaml.FromHex(value);

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override ColorProperty FinishState()
        {
            //
            // 请检查是否有Fallback的显式赋值
            // 请检查是否有需要绑定属性的赋值。
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            return TargetProperty;
        }


        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name);
    }
}