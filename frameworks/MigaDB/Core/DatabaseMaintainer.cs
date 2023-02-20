namespace Acorisoft.FutureGL.MigaDB.Core
{
    public abstract class DatabaseMaintainer<T> : IDatabaseMaintainer where T : class
    {
        protected abstract T OnCreateInstance();

        protected abstract void OnFixInstance(T instance);

        /// <summary>
        /// 是否为无效的数据。
        /// </summary>
        /// <param name="instance">要检查的数据类型。</param>
        /// <returns>如果为无效的数据，则返回True，否则返回False。</returns>
        protected abstract bool IsInvalidated(T instance);

        public void Maintain(IDatabase database)
        {
            //
            // 获得属性集合。
            var props = ((IObjectCollection)database).Props;
            var id = typeof(T).FullName;
            var document = props.FindById(id)?.AsDocument;
            var mapper = BsonMapper.Global;
            T instance;
            
            if (document is null)
            {
                instance = OnCreateInstance();
            }
            else
            {
                instance = mapper.Deserialize<T>(document);
                
                if (IsInvalidated(instance))
                {
                    OnFixInstance(instance);
                }
            }

            //
            // 序列化
            document                           = mapper.Serialize(instance).AsDocument;
            document[Constants.LiteDB_IdField] = id;
            
            //
            // 升级
            props.Upsert(document);
        }
    }
}