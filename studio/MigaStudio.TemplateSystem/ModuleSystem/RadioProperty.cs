using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class RadioPropertyDataView : PropertyDataView<RadioProperty, RadioProperty.Item>
    {
        public RadioPropertyDataView(RadioProperty property) : base(property)
        {            
            Items = new ObservableCollection<RadioProperty.Item>();

            if (property.Items is not null)
            {
                Items.AddRange(property.Items);
            }
        }

        protected override string GenericValueToString(RadioProperty.Item value)
        {
            return string.IsNullOrEmpty(value?.Value) ? TargetProperty.Fallback : value.Value;
        }

        protected override RadioProperty.Item StringToGenericValue(string value) => Items.SingleOrDefault(x => x.Value == value);

        protected override RadioProperty.Item OnValueChanged(RadioProperty.Item oldValue, RadioProperty.Item newValue, out bool fallback)
        {
            if (!Items.Contains(newValue))
            {
                fallback = true;
                return StringToGenericValue(TargetProperty.Fallback);
            }

            fallback = false;
            return newValue;
        }
        
        public ObservableCollection<RadioProperty.Item> Items { get; }
    }

    public class RadioPropertyEditView : PropertyEditView<RadioProperty, RadioProperty.Item>
    {
        public RadioPropertyEditView(RadioProperty property) : base(property)
        {
            Items = new ObservableCollection<RadioProperty.Item>();

            if (property.Items is not null)
            {
                Items.AddRange(property.Items);
            }
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected sealed override bool IsCompleted() => !string.IsNullOrEmpty(Name);

        protected override string ConstructFallback(RadioProperty.Item value) => value.Value;

        protected override RadioProperty.Item DeconstructFallback(string value)=> Items.SingleOrDefault(x => x.Value == value);

        protected override RadioProperty FinishState()
        {
            TargetProperty.Items    = Items.ToArray();
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            return TargetProperty;
        }

        public ObservableCollection<RadioProperty.Item> Items { get; }
    }
}