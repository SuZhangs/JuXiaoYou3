using System.Reactive.Concurrency;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public enum StateChangedEventSource
    {
        Caret,
        TextSource,
        Selection
    }

    public delegate void WorkspaceChangedEventHandler(StateChangedEventSource source, IWorkspace workspace);
    
    public interface IWorkspace : IDisposable
    {
        /// <summary>
        /// 获取当前的文本
        /// </summary>
        string Content { get; }
        IScheduler Scheduler { get; set; }
        WorkspaceChangedEventHandler WorkspaceChanged { get; set; }
        
        void Immutable();
    }

    public abstract class Workspace : ObservableObject, IWorkspace
    {
        private   IScheduler          _scheduler;
        protected DisposableCollector Disposable = new DisposableCollector();

        protected override void ReleaseManagedResources()
        {
            _scheduler = null;
            Disposable.Dispose();
        }

        public abstract void Immutable();

        public IScheduler Scheduler
        {
            get => _scheduler;
            set
            {
                if (value is null)
                {
                    return;
                }

                _scheduler = value;
            }
        }

        public bool IsWorking { get; protected set; }
        public abstract string Content { get; }
        public WorkspaceChangedEventHandler WorkspaceChanged { get; set; }
    }
}