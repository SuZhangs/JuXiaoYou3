namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class Database : Disposable, IDatabase, IObjectCollection
    {
        private readonly string _databaseDirectory;
        private readonly string _databaseFileName;
        private readonly string _databaseIndexFileName;

        private readonly  LiteDatabase                  _database;
        internal readonly ILiteCollection<BsonDocument> _props;

        public Database(LiteDatabase kernel, string root, string fileName, string indexFileName)
        {
            _database              = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _databaseDirectory     = root;
            _databaseFileName      = fileName;
            _databaseIndexFileName = indexFileName;
            _props                 = _database.GetCollection<BsonDocument>(Constants.PropertyCollectionName);


            Property = Get<DatabaseProperty>();
        }

        internal Database(LiteDatabase kernel)
        {
            _database = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _props    = _database.GetCollection<BsonDocument>(Constants.PropertyCollectionName);

            Property = Get<DatabaseProperty>();
        }


        protected override void ReleaseManagedResources()
        {
            _database.Dispose();
        }


        /// <summary>
        /// 获取数据库集合。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <returns>返回一个数据类型。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILiteCollection<T> GetCollection<T>(string collectionName) where T : class => _database.GetCollection<T>(collectionName);

        /// <summary>
        /// 获取指定的集合是否存在。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <returns>返回指定的集合是否存在。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exists(string collectionName) => _database.CollectionExists(collectionName);

        /// <summary>
        /// 获得值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值。</returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T).FullName;
            var document = _props.FindById(key)?.AsDocument;
            return document is null ? default(T) : BsonMapper.Global.Deserialize<T>(document);
        }

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="instance">实例</param>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>返回这个值本身。</returns>
        public T Set<T>(T instance) where T : class
        {
            var key = typeof(T).FullName;
            if (instance is null)
                return default(T);

            var document = BsonMapper.Global.Serialize(instance).AsDocument;
            document[Constants.LiteDB_IdField] = key;
            _props.Upsert(document);
            return instance;
        }

        /// <summary>
        /// 升级版本。
        /// </summary>
        public void UpdateVersion(int version)
        {
            if (version == Version)
            {
                throw new InvalidOperationException(nameof(version));
            }

            //
            // Update
            var information = Get<DatabaseVersion>();
            information.Version        = version;
            information.TimeOfModified = DateTime.Now;

            //
            // Update
            Set(information);
        }

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseFileName => _databaseFileName;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseIndexFileName => _databaseIndexFileName;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseDirectory => _databaseDirectory;

        /// <summary>
        /// 获取当前数据库版本
        /// </summary>
        /// <remarks>操作为非缓存操作，避免数据不一致。</remarks>
        public int Version => Get<DatabaseVersion>().Version;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public DatabaseProperty Property { get; }

        /// <summary>
        /// 
        /// </summary>
        ILiteCollection<BsonDocument> IObjectCollection.Props => _props;
    }
}