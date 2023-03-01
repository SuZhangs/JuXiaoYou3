using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewControllerBase: ViewModelBase, ISplashController
    {
        private          string _caption;
        private readonly object _sync;
        
        protected LaunchViewControllerBase(TabBaseAppViewModel globalParameter)
        {
            _sync   = new object();
            Context = globalParameter;
            Jobs    = new Queue<AsyncJob>(32);
        }

        protected void Job(string id, Action<TabBaseAppViewModel> handler)
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

                    if (job is null || job.Handler is null)
                    {
                        continue;
                    }
                
                    Scheduler.Schedule(() =>
                    {
                        Caption = job.Title;
                    });
                
                    job.Handler(Context);
                }
                
                Scheduler.Schedule(() =>
                {
                    Caption = "加载完成";
                    OnJobCompleted();
                });
                
            }
        }

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
        /// 上下文
        /// </summary>
        public TabBaseAppViewModel Context { get; }
        
        /// <summary>
        /// 所有
        /// </summary>
        public Queue<AsyncJob> Jobs { get; }
    }
}