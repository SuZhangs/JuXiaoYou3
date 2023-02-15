namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public abstract class MonoPropertyDataView<TProperty> : PropertyDataView<TProperty, bool> where TProperty : ModuleProperty
    {
        protected MonoPropertyDataView(TProperty property) : base(property)
        {
            
        }

        protected sealed override string GenericValueToString(bool value) => value.ToString();

        protected sealed override bool StringToGenericValue(string value) => value.ToBoolean(false);
        
        protected sealed override bool OnValueChanged(bool oldValue, bool newValue, out bool fallback)
        {
            fallback = false;
            return newValue;
        }
    }
    
    public abstract class MonoPropertyEditView<TProperty> : PropertyEditView<TProperty, bool> where TProperty : ModuleProperty
    {
        protected MonoPropertyEditView(TProperty property) : base(property)
        {
        }

        protected override string ConstructFallback(bool value) => value.ToString();
        
        protected override bool DeconstructFallback(string value) => value.ToBoolean(false);

        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected sealed override TProperty FinishState()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected sealed override bool IsCompleted() => !string.IsNullOrEmpty(Name);

    }

    public class TalentPropertyDataView : MonoPropertyDataView<TalentProperty>
    {
        public TalentPropertyDataView(TalentProperty property) : base(property)
        {
        }
    }
    
    public class TalentPropertyEditView : MonoPropertyEditView<TalentProperty>
    {
        public TalentPropertyEditView(TalentProperty property) : base(property)
        {
        }
    }
    
    public class FavoritePropertyDataView : MonoPropertyDataView<FavoriteProperty>
    {
        public FavoritePropertyDataView(FavoriteProperty property) : base(property)
        {
        }
    }
    
    public class FavoritePropertyEditView : MonoPropertyEditView<FavoriteProperty>
    {
        public FavoritePropertyEditView(FavoriteProperty property) : base(property)
        {
        }
    }
    
    public class SwitchPropertyDataView : MonoPropertyDataView<SwitchProperty>
    {
        public SwitchPropertyDataView(SwitchProperty property) : base(property)
        {
        }
    }
    
    public class SwitchPropertyEditView : MonoPropertyEditView<SwitchProperty>
    {
        public SwitchPropertyEditView(SwitchProperty property) : base(property)
        {
        }
    }
}