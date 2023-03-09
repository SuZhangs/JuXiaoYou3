using System.Reactive.Concurrency;
using System.Threading;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class LaunchViewControllerBase: ViewModelBase, ITabViewController
    {
        private          string _caption;
        private          object _context;
        private readonly object _sync;
        
        protected LaunchViewControllerBase()
        {
            _sync   = new object();
            Jobs    = new Queue<AsyncJob>(32);
            Init();
        }

        private void Init()
        {
            _context ??= GetExecuteContext();
        }

        protected void Job(string id, Action<object> handler)
        {
            Jobs.Enqueue(new AsyncJob
            {
                Title = Language.GetText(id),
                Handler = handler
            });
        }

        private void AsyncJobProcess()
        {
            lock (_sync)
            {
                while (Jobs.Count > 0)
                {
                    var job = Jobs.Dequeue();

                    if (job?.Handler is null)
                    {
                        continue;
                    }
                
                    Scheduler.Schedule(() =>
                    {
                        Caption = job.Title;
                    });
                
                    job.Handler(_context);
                }
                
                Scheduler.Schedule(() =>
                {
                    Caption = "加载完成";
                    OnJobCompleted();
                });
                
            }
        }

        protected abstract object GetExecuteContext();

        public void Start(ITabViewModel viewModel){}

        protected virtual void OnJobCompleted()
        {
            
        }

        public override void Start()
        {
            ThreadPool.QueueUserWorkItem(_ => AsyncJobProcess());
        }

        /// <summary>
        /// 获取或设置 <see cref="Caption"/> 属性。
        /// </summary>
        public string Caption
        {
            get => _caption;
            set => SetValue(ref _caption, value);
        }
        
        /// <summary>
        /// 所有
        /// </summary>
        public Queue<AsyncJob> Jobs { get; }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id => "::Launch";
    }
}