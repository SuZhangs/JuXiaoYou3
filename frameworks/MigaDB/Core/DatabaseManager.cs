using DryIoc;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;
using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class DatabaseManager : Disposable, IDatabaseManager
    {
        private readonly ObservableProperty<IDatabase>        _database;
        private readonly ObservableProperty<DatabaseProperty> _property;
        private readonly ObservableState                      _isOpen;

        public DatabaseManager()
        {
            _database  = new ObservableProperty<IDatabase>();
            _property  = new ObservableProperty<DatabaseProperty>();
            _isOpen    = new ObservableState();
            Container  = new Container();
            Mediator   = new Mediator(Container.Resolve);
        }

        private async Task<DatabaseResult> LoadImplAsync(string fileName, string indexFileName, bool load = true)
        {
            
        }
        
        public async Task<DatabaseResult> LoadAsync(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DirectoryNotExists);
            }

            
            //
            // 构建文件名
            var databaseCacheFileName = Path.Combine(directory, Constants.DatabaseIndexFileName);
            var databaseFileName = Path.Combine(directory, Constants.DatabaseFileName);

            //
            // 缺失数据库缓存
            if (!File.Exists(databaseCacheFileName))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.MissingFileName);
            }
//
            // 缺失数据库
            if (!File.Exists(databaseFileName))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseNotExists);
            }

            try
            {
                return await LoadImplAsync(databaseCacheFileName, databaseFileName);
            }
            catch
            {
                return DatabaseResult.Failed(DatabaseFailedReason.Unexpected);
            }
        }

        public Task<DatabaseResult> CreateAsync(string directory, DatabaseProperty property)
        {
            throw new NotImplementedException();
        }

        public Task<DatabaseResult> CloseAsync()
        {
            throw new NotImplementedException();
        }

        public TEngine GetEngine<TEngine>() where TEngine : IDataEngine
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDataEngine> GetEngines()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当前打开的引擎。
        /// </summary>
        /// <remarks>
        /// 如果没有打开，则为null。
        /// </remarks>
        public IObservableProperty<IDatabase> Database => _database;

        /// <summary>
        /// 当前打开的引擎。
        /// </summary>
        /// <remarks>
        /// 如果没有打开，则为null。
        /// </remarks>
        public IObservableProperty<DatabaseProperty> Property => _property;

        /// <summary>
        /// 是否加载数据库。
        /// </summary>
        public IObservableState IsOpen => _isOpen;
        
        /// <summary>
        /// Ioc容器，请勿在第三方框架中使用。
        /// </summary>
        public IContainer Container { get; }
        
        /// <summary>
        /// 中介器
        /// </summary>
        public IMediator Mediator { get; }
    }
}