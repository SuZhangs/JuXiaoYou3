using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Exceptions;
using Acorisoft.FutureGL.MigaDB.Core.Maintainers;
using Acorisoft.FutureGL.MigaDB.Core.Migrations;
using DryIoc;
using Acorisoft.FutureGL.MigaDB.Utils;
using MediatR;
using NLog;
using Directory = System.IO.Directory;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class DatabaseManager : Disposable, IDatabaseManager
    {
        /// <summary>
        /// 创建构建器。
        /// </summary>
        /// <returns>返回一个新的构建器实例。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDatabaseManagerBuilder CreateBuilder(ILogger logger)
        {
            return new DatabaseManagerBuilder(logger);
        }

        /// <summary>
        /// 创建默认数据库管理器。
        /// </summary>
        /// <returns>返回一个新的数据库管理器实例。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DatabaseManager GetDefaultDatabaseManager(ILogger logger, DatabaseMode mode)
        {
            return CreateBuilder(logger)
                   .Setup<TemplateEngine>()
                   .Setup<DocumentEngine>()
                   .Setup<ComposeEngine>()
                   .Setup<ImageEngine>(false)
                   .Setup<MusicEngine>(false)
                   .Setup<KeywordEngine>(false)
                   .Maintain<DatabasePresetMaintainer>()
                   .Maintain<DatabasePropertiesMaintainer>()
                   .Maintain<ServicePropertyMaintainer>()
                   .Build(Constants.DatabaseCurrentVersion, mode);
        }

        #region DatabaseManagerBuilder

        internal class GlobalSynchronizer : IDatabaseSynchronizer
        {
            private readonly Action _databaseLoadStarting;
            private readonly Action _databaseLoadCompleted;
            private          int    _pendingCount;

            public GlobalSynchronizer(Action startHandler, Action completedHandler)
            {
                _databaseLoadStarting  = startHandler;
                _databaseLoadCompleted = completedHandler;
                _pendingCount          = 0;
            }

            public void Manual()
            {
                _databaseLoadStarting?.Invoke();
                _databaseLoadCompleted?.Invoke();
            }

            public void Reset()
            {
                Interlocked.Exchange(ref _pendingCount, 0);
            }

            public void Set()
            {
                if (_pendingCount == 0)
                {
                    _databaseLoadStarting?.Invoke();
                }

                Interlocked.Increment(ref _pendingCount);
            }

            public void Unset()
            {
                Interlocked.Decrement(ref _pendingCount);

                if (_pendingCount == 0)
                {
                    _databaseLoadCompleted?.Invoke();
                }
            }
        }

        class DatabaseManagerBuilder : IDatabaseManagerBuilder
        {
            private readonly Container                 _container;
            private readonly List<IDataEngine>         _engines;
            private readonly List<IDatabaseUpdater>    _updaters;
            private readonly List<IDatabaseMaintainer> _maintainers;
            private readonly ILogger                   _logger;
            private readonly object                    _sync;


            private int _engineCount;
            private int _minimumTargetVersion;

            public DatabaseManagerBuilder(ILogger logger)
            {
                _container   = new Container(rules => rules.With(FactoryMethod.ConstructorWithResolvableArguments).WithTrackingDisposableTransients());
                _engines     = new List<IDataEngine>(16);
                _updaters    = new List<IDatabaseUpdater>();
                _maintainers = new List<IDatabaseMaintainer>();
                _sync        = new object();
                _logger      = logger;
            }


            /// <summary>
            /// 注册数据引擎。
            /// </summary>
            /// <typeparam name="TEngine">指定的引擎类型。</typeparam>
            /// <param name="lazyMode">是否为懒加载模式</param>
            /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
            public IDatabaseManagerBuilder Setup<TEngine>(bool lazyMode = true)
                    where TEngine :
                    class,
                    IDataEngine,
                    INotificationHandler<DatabaseCloseNotification>,
                    INotificationHandler<DatabaseOpenNotification>
            {
                if (_container.IsRegistered<TEngine>())
                {
                    return this;
                }

                lock (_sync)
                {
                    if (!lazyMode)
                        Interlocked.Increment(ref _engineCount);

                    var engine = Classes.CreateInstance<TEngine>();

                    if (engine is null)
                        return this;

                    engine.IsLazyMode = lazyMode;
                    _engines.Add(engine);
                    _container.RegisterInstance(engine);
                    _container.UseInstance(typeof(INotificationHandler<DatabaseOpenNotification>), engine, IfAlreadyRegistered.AppendNewImplementation);
                    _container.UseInstance(typeof(INotificationHandler<DatabaseCloseNotification>), engine, IfAlreadyRegistered.AppendNewImplementation);
                }

                return this;
            }

            /// <summary>
            /// 注册升级器。
            /// </summary>
            /// <typeparam name="TMigration">指定的升级器类型。</typeparam>
            /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
            public IDatabaseManagerBuilder Update<TMigration>() where TMigration : class, IDatabaseUpdater
            {
                if (_container.IsRegistered<TMigration>())
                {
                    return this;
                }

                lock (_sync)
                {
                    var updater = Classes.CreateInstance<TMigration>();
                    if (updater is null) return this;

                    if (_updaters.Any(x => x.GetType() == updater.GetType()))
                    {
                        return this;
                    }

                    _minimumTargetVersion = Math.Min(_minimumTargetVersion, updater.TargetVersion);
                    _updaters.Add(updater);
                }

                return this;
            }

            /// <summary>
            /// 注册数据维护工具，用于检测、初始化数据库中的数据。
            /// </summary>
            /// <typeparam name="TMaintainer">指定的维护工具类型。</typeparam>
            /// <returns>返回一个<see cref="IDatabaseManagerBuilder"/></returns>
            public IDatabaseManagerBuilder Maintain<TMaintainer>() where TMaintainer : class, IDatabaseMaintainer
            {
                if (_container.IsRegistered<TMaintainer>())
                {
                    return this;
                }

                lock (_sync)
                {
                    var maintainer = Classes.CreateInstance<TMaintainer>();
                    if (maintainer is null) return this;
                    _maintainers.Add(maintainer);
                }

                return this;
            }

            /// <summary>
            /// 构建引擎。
            /// </summary>
            /// <returns>返回一个<see cref="IDatabaseManager"/></returns>
            public DatabaseManager Build(int databaseVersion, DatabaseMode mode)
            {
                lock (_sync)
                {
                    return new DatabaseManager(
                        _logger, 
                        _container, 
                        _maintainers, 
                        _updaters,
                        _engines,
                        databaseVersion,
                        mode);
                }
            }
        }

        #endregion

        private readonly ObservableProperty<IDatabase>        _database;
        private readonly ObservableProperty<DatabaseProperty> _property;
        private readonly ObservableState                      _isOpen;

        private readonly IReadOnlyList<IDatabaseMaintainer> _maintainers;
        private readonly IReadOnlyList<IDatabaseUpdater>    _updaters;
        private readonly IDatabaseSynchronizer              _synchronizer;
        private readonly IReadOnlyList<IDataEngine>         _engines;
        private readonly int                                _minimumTargetVersion;
        private readonly DatabaseMode                       _databaseMode;
        private readonly ILogger                            _logger;


        public DatabaseManager(ILogger logger,
                               IContainer container, 
                               IReadOnlyList<IDatabaseMaintainer> maintainers,
                               IReadOnlyList<IDatabaseUpdater> updaters,
                               IReadOnlyList<IDataEngine> engines, 
                               int minimumTargetVersion,
                               DatabaseMode mode)
        {
            _logger               = logger;
            _maintainers          = maintainers;
            _engines              = engines;
            _updaters             = updaters;
            _minimumTargetVersion = minimumTargetVersion;
            Container             = container;
            _databaseMode         = mode;
            _database             = new ObservableProperty<IDatabase>();
            _property             = new ObservableProperty<DatabaseProperty>();
            _isOpen               = new ObservableState();
            _synchronizer         = new GlobalSynchronizer(() => { }, () => _isOpen.SetValue(true));
            Mediator              = new Mediator(Container.Resolve);
        }

        private Task MaintainAndUpdate(IDatabase database, int requireMinimumVersion)
        {
            return Task.Run(() =>
            {
                foreach (var maintainer in _maintainers)
                {
                    try
                    {
                        maintainer.Maintain(database);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"维护数据库失败，维护工具为:{maintainer.GetType().FullName}");
                        throw new UpdaterException(ex.Message, ex);
                    }
                }

                var version = database.Version;

                if (requireMinimumVersion > version)
                {
                    _logger.Warn($"正在升级数据库，当前版本为:{version}");

                    foreach (var updater in _updaters)
                    {
                        try
                        {
                            if (updater.TargetVersion >= database.Version)
                            {
                                if (!updater.Update(database))
                                {
                                    _logger.Warn($"数据库升级失败，升级器:{updater.GetType().FullName}");
                                    continue;
                                }

                                if (updater.ResultVersion > database.Version)
                                {
                                    database.UpdateVersion(updater.ResultVersion);
                                    _logger.Warn($"数据库升级完成，当前版本为:{updater.ResultVersion}");
                                }
                                else
                                {
                                    _logger.Error($"升级数据库异常，疑似升级器未手动提升数据库版本，内存版本:{version}，本地版本:{database.Version}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"升级数据库失败，当前版本为:{version}");
                            throw new UpdaterException(ex.Message, ex);
                        }
                    }
                }
            });
        }

        private async Task NotifyEngines(IDatabase database, string directory)
        {
            _isOpen.SetValue(false);
            _synchronizer.Reset();

            //
            // 提示关闭
            await Mediator.Publish(DatabaseCloseNotification.Instance);

            //
            // 提示打开
            await Mediator.Publish(new DatabaseOpenNotification
            {
                Synchronizer = _synchronizer,
                Session = new DatabaseSession
                {
                    DebugMode     = false,
                    Database      = database,
                    RootDirectory = directory,
                    Logger =  _logger
                }
            });

            if (_engines.Count == 0)
            {
                //
                // manual call
                _synchronizer.Manual();
            }

            _isOpen.SetValue(true);
        }

        private async Task<DatabaseResult> LoadImplAsync(string root, string fileName, string indexFileName)
        {
            try
            {
                LiteDatabase  kernel;
                Database database;
                
                if (_databaseMode == DatabaseMode.Attached)
                {
                    var buffer = await File.ReadAllBytesAsync(fileName);
                    var ms     = new MemoryStream(buffer);
                    var log    = new MemoryStream();
                    kernel = new LiteDatabase(ms, BsonMapper.Global, log);
                    database = new Database(kernel, root, fileName, indexFileName, _databaseMode);
                    database.Collect(ms);
                    database.Collect(log);
                }
                else if (_databaseMode == DatabaseMode.Debug)
                {
                    var ms  = new MemoryStream();
                    var log = new MemoryStream();
                    kernel   = new LiteDatabase(ms, BsonMapper.Global, log);
                    database = new Database(kernel, root, fileName, indexFileName, _databaseMode);
                    database.Collect(ms);
                    database.Collect(log);
                }
                else
                {
                    kernel = new LiteDatabase(new ConnectionString
                    {
                        Filename    = fileName,
                        InitialSize = Constants.DatabaseSize
                    });
                    database = new Database(kernel, root, fileName, indexFileName, _databaseMode);
                }

                //
                var property = database.Get<DatabaseProperty>();

                //
                // 关闭之前的数据库
                await CloseAsync();

                //
                // 设置属性
                _property.SetValue(property);

                //
                // 设置数据库
                _database.SetValue(database);

                //
                // 打开
                _isOpen.SetValue(true);

                //
                //
                await MaintainAndUpdate(database, _minimumTargetVersion);

                //
                //
                await NotifyEngines(database, root);

                //
                //
                return DatabaseResult.Successful;
            }
            catch (FileNotFoundException)
            {
                //
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseNotExists);
            }
            catch (IOException)
            {
                // 占用
                return DatabaseResult.Failed(DatabaseFailedReason.Occupied);
            }
            catch (LiteException)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseBroken);
            }
        }

        private async Task<DatabaseResult> CreateImplAsync(string root, DatabaseProperty property, string fileName, string indexFileName)
        {
            try
            {
                var kernel = new LiteDatabase(new ConnectionString
                {
                    Filename    = fileName,
                    InitialSize = Constants.DatabaseSize
                });

                //
                var database = new Database(kernel, root, fileName, indexFileName, _databaseMode);
                
                //
                // 默认设置
                database.Set<DatabaseProperty>(property);

                //
                // 提取属性
                await JSON.ToFileAsync(property, indexFileName);

                //
                // 关闭之前的数据库
                await CloseAsync();

                //
                // 设置属性
                _property.SetValue(property);


                //
                // 设置数据库
                _database.SetValue(database);

                //
                // 打开
                _isOpen.SetValue(true);

                //
                //
                await MaintainAndUpdate(database, _minimumTargetVersion);

                //
                //
                await NotifyEngines(database, root);

                //
                //
                return DatabaseResult.Successful;
            }
            catch (FileNotFoundException)
            {
                //
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseNotExists);
            }
            catch (IOException)
            {
                // 占用
                return DatabaseResult.Failed(DatabaseFailedReason.Occupied);
            }
            catch (LiteException)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseBroken);
            }
        }

        /// <summary>
        /// 加载指定的世界观
        /// </summary>
        /// <param name="directory">世界观目录</param>
        /// <returns>返回一个操作结果</returns>
        public async Task<DatabaseResult> LoadAsync(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DirectoryNotExists);
            }
            
            if (!Directory.Exists(directory) && _databaseMode != DatabaseMode.Debug)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DirectoryNotExists);
            }



            //
            // 构建文件名
            var databaseCacheFileName = Path.Combine(directory, Constants.DatabaseIndexFileName);
            var databaseFileName      = Path.Combine(directory, Constants.DatabaseFileName);

            //
            // 缺失数据库缓存
            if (!File.Exists(databaseCacheFileName) && _databaseMode != DatabaseMode.Debug)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.MissingFileName);
            }

            //
            // 缺失数据库
            if (!File.Exists(databaseFileName) && _databaseMode != DatabaseMode.Debug)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseNotExists);
            }

            try
            {
                return await LoadImplAsync(directory, databaseFileName, databaseCacheFileName);
            }
            catch
            {
                return DatabaseResult.Failed(DatabaseFailedReason.Unexpected);
            }
        }

        /// <summary>
        /// 加载指定的世界观
        /// </summary>
        /// <param name="directory">世界观目录</param>
        /// <param name="property">世界观属性</param>
        /// <returns>返回一个操作结果</returns>
        public async Task<DatabaseResult> CreateAsync(string directory, DatabaseProperty property)
        {
            if (!Directory.Exists(directory))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DirectoryNotExists);
            }

            if (!DatabaseProperty.IsValid(property))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.MissingParameter);
            }

            var targetFileName      = Path.Combine(directory, Constants.DatabaseFileName);
            var targetIndexFileName = Path.Combine(directory, Constants.DatabaseIndexFileName);

            if (File.Exists(targetFileName))
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseAlreadyExists);
            }

            try
            {
                return await CreateImplAsync(directory, property, targetFileName, targetIndexFileName);
            }
            catch
            {
                return DatabaseResult.Failed(DatabaseFailedReason.Unexpected);
            }
        }

        /// <summary>
        /// 关闭世界观。
        /// </summary>
        /// <returns>返回一个操作结果</returns>
        public async Task<DatabaseResult> CloseAsync()
        {
            if (!_isOpen.CurrentValue)
            {
                return DatabaseResult.Successful;
            }

            //
            // Update
            var ver = _database.CurrentValue.Get<DatabaseVersion>();
            ver.TimeOfModified = DateTime.Now;
            _database.CurrentValue.Set(ver);

            //
            // 写入文件
            if (_property.CurrentValue is not null)
            {
                await JSON.ToFileAsync(_property.CurrentValue, _database.CurrentValue.DatabaseIndexFileName);
            }

            //
            // 关闭
            await Mediator.Publish(DatabaseCloseNotification.Instance);

            // 释放
            _database.Dispose();

            //
            // 关闭
            _property.SetValue(null);
            _isOpen.SetValue(false);
            _database.SetValue(null);

            //
            //
            return DatabaseResult.Successful;
        }

        /// <summary>
        /// 保存当前数据库的对象更改。
        /// </summary>
        /// <returns>返回一个操作结果</returns>
        public async Task<DatabaseResult> CacheAsync()
        {
            if (!_isOpen.CurrentValue)
            {
                return DatabaseResult.Failed(DatabaseFailedReason.DatabaseNotOpen);
            }

            var database = _database.CurrentValue;
            var indexFile = database.DatabaseIndexFileName;
            var property = database.Get<DatabaseProperty>();

            try
            {
                await JSON.ToFileAsync(property, indexFile);
                return DatabaseResult.Successful;
            }
            catch
            {
                return DatabaseResult.Failed(DatabaseFailedReason.Unexpected);
            }
        }
 
        /// <summary>
        /// 获取引擎。
        /// </summary>
        /// <typeparam name="TEngine">指定的引擎类型。</typeparam>
        /// <returns>返回指定的引擎类型（如果存在），否则返回null。</returns>
        public TEngine GetEngine<TEngine>() where TEngine : IDataEngine
        {
            var engine = Container.Resolve<TEngine>();
            if (!engine.Activated)
            {
                engine.Activate();
            }

            return engine;
        }

        /// <summary>
        /// 获得所有数据引擎。
        /// </summary>
        /// <returns>返回一个数据引擎的枚举。</returns>
        public IEnumerable<IDataEngine> GetEngines() => _engines;

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