using System.Collections.ObjectModel;
using Newtonsoft.Json;
using IRepoOpenHandler = MediatR.INotificationHandler<Acorisoft.Miga.Doc.Core.RepoOpenMessage>;
using IRepoSetHandler = MediatR.INotificationHandler<Acorisoft.Miga.Doc.Core.RepoSetMessage>;
using IRepoCloseHandler = MediatR.INotificationHandler<Acorisoft.Miga.Doc.Core.RepoCloseMessage>;

// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

namespace Acorisoft.Miga.Doc.Core
{
    public partial class RepositoryEngine : Disposable, IRepositoryEngine
    {
        private readonly Mediator              _mediator;
        private readonly Container             _ioc;
        private readonly IList<StorageService> _services;
        private readonly object                _sync;
        private volatile bool                  _isEngineLoading;


        private RepositoryProperty      _property;
        private RepositoryInformation   _information;
        private RepositoryConfiguration _configuration;
        private RepositoryAuthor        _author;

        private LiteDatabase      _database;
        private ObjectManager     _objectManager;
        private string            _repositoryFolderName;
        private string            _repositoryEntryFileName;
        private RepositoryContext _repositoryContext;


        public RepositoryEngine()
        {
            _ioc      = new Container();
            _mediator = new Mediator(_ioc.Resolve);
            _services = new List<StorageService>(32);
            _sync     = new object();
            Status    = new RepositoryStatusEngine();

            RegisterModules();
        }


        public static string GetIndexFileName(string folderName)
        {
            return Path.Combine(folderName, Constants.index_file);
        }

        #region RegisterModules / RegisterInstance

        private partial void RegisterModules();

        private void RegisterInstance(StorageService sc)
        {
            _ioc.RegisterInstance(sc.GetType(), sc);
            _ioc.UseInstance(typeof(IRepoOpenHandler), sc, IfAlreadyRegistered.AppendNewImplementation);
            _ioc.UseInstance(typeof(IRepoSetHandler), sc, IfAlreadyRegistered.AppendNewImplementation);
            _ioc.UseInstance(typeof(IRepoCloseHandler), sc, IfAlreadyRegistered.AppendNewImplementation);
            _services.Add(sc);
        }

        public bool IsRegistered<T>() => _ioc.IsRegistered(typeof(T));

        #endregion

        #region LoadAsync / CloseAsync / CreateAsync / SaveAsync

        private void Save()
        {
            var payload = JsonConvert.SerializeObject(_configuration);
            File.WriteAllText(_repositoryEntryFileName!, payload);
            
            _objectManager.SetObject(_information);
            _objectManager.SetObject(_property);
            _objectManager.SetObject(_author);
        }

        public Task SaveAsync()
        {
            return Task.Run(Save);
        }

        private async Task<IsCompleted<string>> CreateAsyncImpl(string folderName, RepositoryProperty property)
        {
            property ??= new RepositoryProperty
            {
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection(),
            };

            var information = new RepositoryInformation
            {
                Version = 1
            };

            var indexFileName = GetIndexFileName(folderName);
            var configuration = new RepositoryConfiguration();
            if (File.Exists(indexFileName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryExists
                };
            }

            MaintainingRepositoryConfiguration(configuration, property);

            var payload = JsonConvert.SerializeObject(configuration);
            await File.WriteAllTextAsync(indexFileName!, payload);

            _configuration = configuration;
            return await LoadAsyncImpl(folderName, configuration, property, information, true);
        }

        private async Task<IsCompleted<string>> LoadAsyncImpl(string folderName)
        {
            var indexFileName = GetIndexFileName(folderName);

            if (!File.Exists(indexFileName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryNotExists
                };
            }

            try
            {
                var payload = await File.ReadAllTextAsync(indexFileName);
                var configuration = JsonConvert.DeserializeObject<RepositoryConfiguration>(payload)!;
                return await LoadAsyncImpl(folderName, configuration, null, null, false);
            }
            catch
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryFormatIsBad
                };
            }
        }

        private async Task<IsCompleted<string>> LoadAsyncImpl(
            string folderName,
            RepositoryConfiguration configuration,
            RepositoryProperty property,
            RepositoryInformation information,
            bool created)
        {
            var indexFileInfo = new DirectoryInfo(folderName);
            var baseDirectory = indexFileInfo.FullName;

            var documentDatabaseFileName = Path.Combine(baseDirectory, Constants.main_database);

            lock (_sync)
            {
                _database                = new LiteDatabase(documentDatabaseFileName);
                _repositoryEntryFileName = GetIndexFileName(folderName);
                _repositoryFolderName    = baseDirectory;
                _property                = property;
                _objectManager           = new ObjectManager(_database);

                if (created)
                {
                    _information = _objectManager.SetObject(information);
                    _property    = _objectManager.SetObject(property);
                }
                else
                {
                    _property    = _objectManager.GetObject<RepositoryProperty>();
                    _information = _objectManager.GetObject<RepositoryInformation>();
                    _author      = _objectManager.GetObject<RepositoryAuthor>();
                }


                if (string.IsNullOrEmpty(_property.IndexId)) _property.IndexId = ShortGuidString.GetId();
                _configuration = configuration;
                
                //
                // maintaining
                MaintainingRepositoryConfiguration(_configuration, _property);
                MaintainRepositoryAttachProperty();
                
                //
                // context
                _repositoryContext = new RepositoryContext
                {
                    Database              = _database,
                    RepositoryFolder      = _repositoryFolderName,
                    StatusEngine          = Status,
                    RepositoryInformation = _information
                };

                IsLoaded = true;
            }


            await _mediator.Publish(new RepoCloseMessage
            {
                Context = new RepositoryContext
                {
                    StatusEngine = Status
                }
            });

            await _mediator.Publish(new RepoOpenMessage
            {
                Context  = _repositoryContext,
                Property = _property
            });

            return new IsCompleted<string>
            {
                IsFinished = true,
                Result     = _property.Id,
            };
        }

        /// <summary>
        /// 创建一个新的世界观
        /// </summary>
        /// <param name="folderName">世界观保存的位置</param>
        /// <param name="property">世界观属性</param>
        /// <returns>返回当前操作的结果。</returns>
        public async Task<IsCompleted<string>> CreateAsync(string folderName, RepositoryProperty property)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryNotExists
                };
            }

            if (string.IsNullOrEmpty(property.Name))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = string.Format(DocSR.ErrMsg_RepositoryNotExists, nameof(property.Name))
                };
            }

            if (string.IsNullOrEmpty(property.Author))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = string.Format(DocSR.ErrMsg_RepositoryNotExists, nameof(property.Author))
                };
            }

            if (string.IsNullOrEmpty(property.EnglishName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = string.Format(DocSR.ErrMsg_RepositoryNotExists, nameof(property.Email))
                };
            }

            return await CreateAsyncImpl(folderName, property);
        }

        /// <summary>
        /// 创建一个新的世界观
        /// </summary>
        /// <param name="folderName">世界观保存的位置</param>
        /// <param name="name">世界观名称</param>
        /// <param name="ownerName">作者名</param>
        /// <param name="avatar">世界观头像</param>
        /// <param name="email">作者邮箱</param>
        /// <returns>返回当前操作的结果。</returns>
        public async Task<IsCompleted<string>> CreateAsync(
            string folderName,
            string name,
            string ownerName,
            string avatar,
            string email)
        {
            return await CreateAsync(folderName, new RepositoryProperty
            {
                Name   = name,
                Avatar = avatar,
                Email  = email,
                Author = ownerName
            });
        }

        /// <summary>
        /// 关闭一个新的世界观
        /// </summary>
        public async Task<IsCompleted<string>> CloseAsync()
        {
            try
            {
                // ReSharper disable once InvertIf
                if (IsLoaded)
                {
                    _database.Checkpoint();
                    _database.Checkpoint();

                    Save();

                    lock (_sync)
                    {
                        _isEngineLoading = false;

                        _database?.Dispose();
                        _objectManager           = null;
                        _property                = null;
                        _information             = null;
                        _configuration           = null;
                        _repositoryEntryFileName = null;
                        _repositoryFolderName    = null;
                        _repositoryContext       = null;
                    }

                    await _mediator.Publish(new RepoCloseMessage
                    {
                        Context = new RepositoryContext
                        {
                            StatusEngine = Status
                        }
                    });
                }

                return new IsCompleted<string> { IsFinished = true };
            }
            catch (Exception ex)
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisposeImpl()
        {
            try
            {
                // ReSharper disable once InvertIf
                if (IsLoaded)
                {
                    var payload = JsonConvert.SerializeObject(_property);
                    File.WriteAllText(_repositoryEntryFileName!, payload);

                    lock (_sync)
                    {
                        _database.Checkpoint();
                        _database.Checkpoint();
                        Status._isEngineLoading.OnNext(false);

                        _database?.Dispose();
                        _repositoryEntryFileName = null;
                        _repositoryFolderName    = null;
                        _repositoryContext       = null;
                        _ioc.Dispose();
                    }
                }
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 加载一个世界观
        /// </summary>
        /// <param name="folderName">世界观保存的位置</param>
        /// <returns>返回当前操作的结果。</returns>
        public async Task<IsCompleted<string>> LoadAsync(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryNotExists
                };
            }

            if (!Directory.Exists(folderName))
            {
                return new IsCompleted<string>
                {
                    IsFinished = false,
                    Message    = DocSR.ErrMsg_RepositoryNotExists
                };
            }

            return await LoadAsyncImpl(folderName);
        }

        #endregion

        #region Maintaining

        private void MaintainRepositoryAttachProperty()
        {
            if (_author is null)
            {
                var name = $"{_property.Author}【{DocSR.Text_Author}】";

                _author = new RepositoryAuthor
                {
                    Name   = name,
                    Avatar = _property.Avatar
                };
                _objectManager.SetObject(_author);
            }
        }


        private static void MaintainingRepositoryConfiguration(RepositoryConfiguration configuration, RepositoryProperty property)
        {
            if (property is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(property.Id))
            {
                property.Id = ShortGuidString.GetId();
            }
            
            property.Backgrounds    ??= new ObservableCollection<string>();
            property.Parts          ??= new DataPartCollection();
            property.Metas          ??= new MetadataCollection();
            configuration.Assets    ??= new ObservableCollection<CustomMetadataView>();
            configuration.Character ??= new ObservableCollection<CustomMetadataView>();
            configuration.Custom    ??= new ObservableCollection<CustomMetadataView>();
            configuration.Materials ??= new ObservableCollection<CustomMetadataView>();
            configuration.Maps      ??= new ObservableCollection<CustomMetadataView>();
            configuration.Skills    ??= new ObservableCollection<CustomMetadataView>();
            DeleteInvalidateCustomMetadataView(configuration.Assets);
            DeleteInvalidateCustomMetadataView(configuration.Character);
            DeleteInvalidateCustomMetadataView(configuration.Maps);
            DeleteInvalidateCustomMetadataView(configuration.Materials);
            DeleteInvalidateCustomMetadataView(configuration.Custom);
            DeleteInvalidateCustomMetadataView(configuration.Skills);
        }

        private static void DeleteInvalidateCustomMetadataView(ObservableCollection<CustomMetadataView> collection)
        {
            if (collection is null)
            {
                return;
            }

            for (var i = 0; i < collection.Count; i++)
            {
                var item = collection[i];
                if (string.IsNullOrEmpty(item.Metadata) ||
                    string.IsNullOrEmpty(item.Color) ||
                    string.IsNullOrEmpty(item.Title))
                {
                    collection.RemoveAt(i);
                }
            }
        }

        #endregion

        protected override void ReleaseUnmanagedResources()
        {
            DisposeImpl();
        }

        #region GetService / GetServices

        


        public T GetService<T>() where T : StorageService
        {
            return _ioc.Resolve<T>();
        }

        public IEnumerable<StorageService> GetServices() => _services;
        
        #endregion


        /// <summary>
        /// 获取或设置当前仓储引擎的加载状态。
        /// </summary>
        public bool IsLoaded
        {
            get => _isEngineLoading;
            private set
            {
                _isEngineLoading = value;
                Status._isEngineLoading.OnNext(value);
            }
        }

        /// <summary>
        /// 获取或设置当前仓储状态引擎。
        /// </summary>
        public RepositoryStatusEngine Status { get; }

        /// <summary>
        /// 获取当前世界观的属性
        /// </summary>
        public RepositoryProperty Property => _property;

        /// <summary>
        /// 获取当前仓储的信息
        /// </summary>
        public RepositoryInformation Information => _information;

        /// <summary>
        /// 获取当前仓储的配置
        /// </summary>
        public RepositoryConfiguration Configuration => _configuration;

        /// <summary>
        /// 获取当前仓储的作者信息
        /// </summary>
        public RepositoryAuthor Author => _author;

        /// <summary>
        /// 获取当前的工作路径
        /// </summary>
        public string RepositoryFolder => _repositoryFolderName;

        /// <summary>
        /// 
        /// </summary>
        public ObjectManager ObjectManager => _objectManager;
    }
}