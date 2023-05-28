namespace Acorisoft.Miga.Doc.Documents
{
    public abstract class MetaItem : ObservableObject
    {
        protected MetaItem(string propertyName) => PropertyName = propertyName;

        internal void Update(Metadata metadata)
        {
            OnUpdate(metadata?.Value ?? string.Empty);
        }

        protected virtual void OnUpdate(string value)
        {
        }

        public string PropertyName { get; }
    }

    public class MetaItem<T> : MetaItem
    {
        private T _value;

        public MetaItem(string propertyName) : base(propertyName)
        {
        }

        protected override void OnUpdate(string value)
        {
            _value = StringConverter.Convert<T>(value);
            RaiseUpdated(nameof(Value));
        }

        public T Value => _value;
    }

    public class StringMetaItem : MetaItem<string>
    {
        internal StringMetaItem() : base(nameof(Value))
        {
        }
        public string Metadata { get; init; }
        public string MainTitle { get; init; }
        public string Color { get; init; }
    }
}