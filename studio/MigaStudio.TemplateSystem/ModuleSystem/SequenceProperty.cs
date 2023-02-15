using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public class SequencePropertyDataView : PropertyDataView<SequenceProperty, SequenceProperty.Item>
    {
        public SequencePropertyDataView(SequenceProperty property) : base(property)
        {            
            Items = new ObservableCollection<SequenceProperty.Item>();

            if (property.Items is not null)
            {
                Items.AddRange(property.Items);
            }
        }

        protected override string GenericValueToString(SequenceProperty.Item value)
        {
            return string.IsNullOrEmpty(value?.Name) ? TargetProperty.Fallback : value.Name;
        }

        protected override SequenceProperty.Item StringToGenericValue(string value) => Items.SingleOrDefault(x => x.Name == value);

        protected override SequenceProperty.Item OnValueChanged(SequenceProperty.Item oldValue, SequenceProperty.Item newValue, out bool fallback)
        {
            if (!Items.Contains(newValue))
            {
                fallback = true;
                return StringToGenericValue(TargetProperty.Fallback);
            }

            fallback = false;
            return newValue;
        }
        
        public ObservableCollection<SequenceProperty.Item> Items { get; }
    }

    public class SequencePropertyEditView : PropertyEditView<SequenceProperty, SequenceProperty.Item>
    {
        public SequencePropertyEditView(SequenceProperty property) : base(property)
        {
            Items = new ObservableCollection<SequenceProperty.Item>();

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

        protected override string ConstructFallback(SequenceProperty.Item value) => value.Name;

        protected override SequenceProperty.Item DeconstructFallback(string value)=> Items.SingleOrDefault(x => x.Name == value);

        protected override SequenceProperty FinishState()
        {
            TargetProperty.Items    = Items.ToArray();
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);
            return TargetProperty;
        }

        public ObservableCollection<SequenceProperty.Item> Items { get; }
    }
}