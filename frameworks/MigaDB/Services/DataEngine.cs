using MediatR;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public abstract class DataEngine :
        IDataEngine, 
        INotificationHandler<DatabaseOpenNotification> ,
        INotificationHandler<DatabaseCloseNotification>
    {
        private DatabaseOpenNotification _notification;
        
        #region Handler

        public Task Handle(DatabaseOpenNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (IsLazyMode)
                    {
                        _notification = notification;
                        return;
                    }
                    
                    //
                    // 加载
                    notification.Synchronizer.Set();
                    OnDatabaseOpening(notification.Session);
                    notification.Synchronizer.Unset();
                }
                catch
                {
                    //
                }
            }, cancellationToken);
        }

        public Task Handle(DatabaseCloseNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    OnDatabaseClosing();
                }
                catch
                {
                    //
                }

                Activated     = false;
                _notification = null;
                
            }, cancellationToken);
        }

        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        public virtual void Refresh(){}
        
        /// <summary>
        /// 清空
        /// </summary>
        public virtual void Clear(){}

        /// <summary>
        /// 测试开启
        /// </summary>
        /// <param name="session"></param>
        internal void DatabaseOpeningForTesting(DatabaseSession session) => OnDatabaseOpening(session);

        /// <summary>
        /// 测试关闭
        /// </summary>
        internal void DatabaseClosingForTesting() => OnDatabaseClosing();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        protected abstract void OnDatabaseOpening(DatabaseSession session);
        
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnDatabaseClosing();

        /// <summary>
        /// 手动计划数据引擎
        /// </summary>
        /// <returns>按返回操作的结果。</returns>
        public bool Activate()
        {
            if (!Activated)
            {
                try
                {
                    OnDatabaseOpening(_notification.Session);
                    _notification = null;
                    Activated     = true;
                }
                catch (Exception e)
                {
                    //
                    throw;
                }
            }

            return Activated;
        }
        
        /// <summary>
        /// 是否为懒加载模式
        /// </summary>
        public bool IsLazyMode { get; set; }
        
        /// <summary>
        /// 是否已经加载。
        /// </summary>
        public bool Activated { get; private set; }
    }
}