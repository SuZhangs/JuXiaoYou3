namespace Acorisoft.Miga.Doc.Documents
{
    public class DocumentPartManager
    {
        private readonly Dictionary<Type, DataPart> _mappingByType;

        private Document _document;

        public DocumentPartManager()
        {
            _mappingByType = new Dictionary<Type, DataPart>(7);
        }


        public void Unmanaged()
        {
            _document = null;

            _mappingByType.Clear();
        }

        public void Manage(Document document)
        {
            if (document is null)
            {
                return;
            }

            _document = document;

            //
            // 添加模组
            foreach (var part in document.Parts.Where(part => part is not null))
            {
                _mappingByType.TryAdd(part.GetType(), part);
            }
        }


        public T GetPart<T>() where T : DataPart
        {
            // ReSharper disable once InvertIf
            if (!_mappingByType.TryGetValue(typeof(T), out var dataPart))
            {
                dataPart = Classes.CreateInstance<T>();
                _mappingByType.TryAdd(typeof(T), dataPart);
                _document.Parts.Add(dataPart);
            }

            return (T)dataPart;
        }
    }
}