﻿using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    /// <summary>
    /// <see cref="IViewModel"/> 表示一个视图模型基类。
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// 首次启动
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        Task Start();

        /// <summary>
        /// 表示关闭
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        void Stop();

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
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        void Start(ViewParam arg);
        
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