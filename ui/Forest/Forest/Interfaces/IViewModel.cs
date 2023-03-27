using System.Reactive.Concurrency;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IViewModel"/> 表示一个视图模型基类。
    /// </summary>
    public interface IViewModel : ILifetimeSupport
    {
        
        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        void Startup(Parameter arg);

        /// <summary>
        /// 表示挂起
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        void Suspend();
        
        
        /// <summary>
        /// 表示恢复
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        void Resume();
        
        /// <summary>
        /// 获得调度器
        /// </summary>
        IScheduler Scheduler { get; }
        
        /// <summary>
        /// 获得垃圾回收器
        /// </summary>
        DisposableCollector Collector { get; }
    }
}