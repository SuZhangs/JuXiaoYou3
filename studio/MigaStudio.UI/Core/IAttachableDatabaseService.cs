using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IAttachableDatabaseService
    {
        void Start(IDatabaseManager databaseManager);
        void Register(IContainer container);
        void Unregister(IContainer container);
        void Stop();
    }

    public interface IAttachableDatabaseServiceHost
    {
        void Add(IAttachableDatabaseService service);
        void Remove(IAttachableDatabaseService service);
    }

    public class AttachableDatabaseServiceHost : Disposable, IAttachableDatabaseServiceHost
    {
        private readonly IContainer                       _container;
        private readonly IDatabaseManager                 _databaseManager;
        private readonly List<IAttachableDatabaseService> _services;
        private readonly IDisposable                      _disposable;

        public AttachableDatabaseServiceHost(IContainer container, IDatabaseManager databaseManager)
        {
            _container       = container;
            _databaseManager = databaseManager;
            _services        = new List<IAttachableDatabaseService>(16);
            _disposable = _databaseManager.IsOpen
                                          .Observable
                                          .Subscribe(OnDatabaseOpenStateChanged);
            
            container.Use<AttachableDatabaseServiceHost, IAttachableDatabaseServiceHost>(this);
        }

        protected override void ReleaseManagedResources()
        {
            _disposable.Dispose();
        }

        private void OnDatabaseOpenStateChanged(bool x)
        {
            if (x)
            {
                _services.ForEach(a => a.Start(_databaseManager));
            }
            else
            {
                _services.ForEach(a => a.Stop());
            }
        }

        public void Add(IAttachableDatabaseService service)
        {
            if (service is null)
            {
                return;
            }
            service.Register(_container);
            _services.Add(service);
        }

        public void Remove(IAttachableDatabaseService service)
        {
            if (service is null)
            {
                return;
            }
            
            service.Unregister(_container);
            _services.Remove(service);
        }
    }
}