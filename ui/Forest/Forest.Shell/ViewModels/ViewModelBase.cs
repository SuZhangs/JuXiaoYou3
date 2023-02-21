using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> 表示一个视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : ForestObject, IViewModel, IViewModelLanguageService
    {
        private Dictionary<string, ILanguageNode> _elements;
        private string                            _rootName;
        
        protected ViewModelBase()
        {
            Collector = new DisposableCollector(8);
        }
        
        /// <summary>
        /// 首次启动
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        public virtual Task Start()
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="param">视图参数</param>
        public virtual void Start(ViewParam param)
        {
            
        }
        /// <summary>
        /// 表示关闭
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        public virtual void Stop()
        {
            
        }

        /// <summary>
        /// 表示挂起
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        public virtual void Suspend()
        {
            
        }
        
        /// <summary>
        /// 表示恢复
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        public virtual void Resume()
        {
            
        }

        /// <summary>
        /// 获得调度器
        /// </summary>
        public IScheduler Scheduler => Xaml.Get<IScheduler>();
        

        /// <summary>
        /// 获得垃圾回收器
        /// </summary>
        public DisposableCollector Collector { get; }
        
        string IViewModelLanguageService.RootName
        {
            get => _rootName;
            set => _rootName = value;
        }
    }
}