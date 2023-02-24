﻿using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> 表示一个视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : ForestObject, IViewModel, IViewModelLanguageService
    {
        private readonly Dictionary<string, IRelayCommand> _commandMapping;

        private string _rootName;

        protected ViewModelBase()
        {
            _commandMapping = new Dictionary<string, IRelayCommand>();
            Collector       = new DisposableCollector(8);
        }

        #region Commands

        protected AsyncRelayCommand AsyncCommand(Func<Task> execute) => new AsyncRelayCommand(execute);

        protected AsyncRelayCommand AsyncCommand(Func<Task> execute, Func<bool> canExecute) => new AsyncRelayCommand(execute, canExecute);

        protected AsyncRelayCommand<T> AsyncCommand<T>(Func<T, Task> execute) => new AsyncRelayCommand<T>(execute);

        protected AsyncRelayCommand<T> AsyncCommand<T>(Func<T, Task> execute, Predicate<T> canExecute) => new AsyncRelayCommand<T>(execute, canExecute);

        protected RelayCommand Command(Action execute) => new RelayCommand(execute);

        protected RelayCommand Command(Action execute, Func<bool> canExecute) => new RelayCommand(execute, canExecute);

        protected TCommand Associate<TCommand>(string key, TCommand command) where TCommand : IRelayCommand
        {
            _commandMapping.TryAdd(key, command);
            return command;
        }
        
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            if (_commandMapping.TryGetValue(propertyName, out var command))
            {
                command?.NotifyCanExecuteChanged();
            }
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