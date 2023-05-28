namespace Acorisoft.Miga.Doc.Core
{
    public class ObjectManager
    {
        private readonly ILiteCollection<BsonDocument> _doc;
        
        internal ObjectManager(ILiteDatabase database)
        {
            _doc = database.GetCollection<BsonDocument>(Constants.cn_prop);
        }

        public T GetObject<T>() where T : class
        {
            var key = typeof(T).FullName;
            if (_doc.Exists(Query.EQ(Constants.fieldName_id, key)))
            {
                var document = _doc.FindById(key);
                 return BsonMapper.Global.Deserialize<T>(document);
            }

            return default(T);
        }
        
        public T SetObject<T>(T instance) where T : class
        {
            var key = typeof(T).FullName;
            var doc = BsonMapper.Global.Serialize(instance).AsDocument;
            doc[Constants.fieldName_id] = key;
            _doc.Upsert(key, doc);

            return instance;
        }
    }
}