using Newtonsoft.Json;

namespace Acorisoft.Miga.Doc.Engines
{
    using IRepoOpenHandler = INotificationHandler<RepoOpenMessage>;
    using IRepoSetHandler = INotificationHandler<RepoSetMessage>;
    using IRepoCloseHandler = INotificationHandler<RepoCloseMessage>;


    /// <summary>
    /// <see cref="StorageService"/> 类型表示一个数据库存储集合。
    /// </summary>
    public abstract class StorageService : IRepoOpenHandler, IRepoSetHandler, IRepoCloseHandler
    {
        private volatile bool            _initialized;
        private          RepoOpenMessage _cache;

        #region Override Methods

        protected internal abstract void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property);
        protected internal abstract void OnRepositoryClosing(RepositoryContext context);

        protected internal virtual void OnRepositorySetting(UserCredential credential)
        {
        }

        protected static T DeserializeFromBase64<T>(string base64)
        {
            var buffer = Convert.FromBase64String(base64);
            var payload = Encoding.UTF8.GetString(buffer);
            return JsonConvert.DeserializeObject<T>(payload);
        }

        protected static string SerializeToBase64(object instance)
        {
            var payload = JsonConvert.SerializeObject(instance);
            var buffer = Encoding.UTF8.GetBytes(payload);
            return Convert.ToBase64String(buffer);
        }
        
        #endregion


        private void RepositorySetting(UserCredential credential)
        {
            Credential = credential;
            OnRepositorySetting(credential);
        }

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="directory">要检查的目录</param>
        /// <returns>返回目录路径。</returns>
        protected static string CheckDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                return string.Empty;
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        #region INotificationHandlers

        Task IRepoOpenHandler.Handle(RepoOpenMessage notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var status = notification.Context.StatusEngine;
                status.Enqueue();
                IsDisposed = false;
                if (IsLazyMode)
                {
                    if (_initialized)
                    {
                        OnRepositoryOpening(notification.Context, notification.Property);
                    }
                    else
                    {
                        _cache = notification;
                    }
                }
                else
                {
                    OnRepositoryOpening(notification.Context, notification.Property);
                }

                status.Dequeue();
            }, cancellationToken);
        }

        Task IRepoSetHandler.Handle(RepoSetMessage notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => { RepositorySetting(notification.Credential); }, cancellationToken);
        }

        Task IRepoCloseHandler.Handle(RepoCloseMessage notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var status = notification.Context.StatusEngine;
                status.Enqueue();
                OnRepositoryClosing(notification.Context);
                status.Dequeue();
                _cache     = null;
                IsDisposed = true;
            }, cancellationToken);
        }

        #endregion

        /// <summary>
        /// 手动初始化，适合懒加载模式。
        /// </summary>
        /// <remarks>此操作有效能防止启动卡顿，在低配电脑上效果显著。但是如果硬盘IO本身就非常低，这个优化是效果甚微。</remarks>
        public bool ManualInitialized()
        {
            if (!IsLazyMode)
            {
                return false;
            }

            if (_cache is null)
            {
                return false;
            }

            if (_initialized)
            {
                return false;
            }

            //
            //
            OnRepositoryOpening(_cache.Context, _cache.Property);
            _initialized = true;
            return true;
        }

        /// <summary>
        /// 用户凭证
        /// </summary>
        public UserCredential Credential { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLazyMode { get; protected set; }

        public bool IsInitialized => _initialized;

        public bool IsDisposed { get; private set; }
    }

    public static class NoSqlUtils
    {
        public static bool Contains<T>(this ILiteCollection<T> collection, BsonValue value)
        {
            return collection.Exists(Query.EQ("_id", value));
        }

        public static T FindByName<T>(this ILiteCollection<T> collection, BsonValue value)
        {
            return collection.FindOne(Query.EQ("Name", value));
        }

        public static bool ContainName<T>(this ILiteCollection<T> collection, BsonValue value)
        {
            return collection.Exists(Query.EQ("Name", value));
        }

        public static bool Contains<T>(this ILiteCollection<T> collection, string field, BsonValue value)
        {
            return collection.Exists(Query.EQ(field, value));
        }

        public static T FindOne<T>(this ILiteCollection<T> collection, string field, BsonValue value)
        {
            return collection.FindOne(Query.EQ(field, value));
        }


        public static bool AlwaysTrue<T>(T _) => true;
    }
}