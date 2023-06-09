using System.Reactive.Concurrency;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public interface IWorkspace
    {
        /// <summary>
        /// 获取当前的文本
        /// </summary>
        string Content { get; }
        IScheduler Scheduler { get; set; }

        event EventHandler PositionChanged;
        event EventHandler TextChanged;
        event EventHandler SelectionChanged;
        void Immutable();
    }

    public abstract class Workspace : ObservableObject, IWorkspace
    {
        private            IScheduler           _scheduler;
        protected readonly DisposableCollector Disposable = new DisposableCollector();

        protected override void ReleaseManagedResources()
        {
            _scheduler = null;
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
        public event EventHandler PositionChanged;
        public event EventHandler TextChanged;
        public event EventHandler SelectionChanged;
    }
}