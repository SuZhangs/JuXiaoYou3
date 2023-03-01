using System.ComponentModel;
using System.Reactive.Concurrency;
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
        private readonly List<IRelayCommand> _commandMapping;

        private string _rootName;

        protected ViewModelBase()
        {
            _commandMapping = new List<IRelayCommand>(8);
            Collector       = new DisposableCollector(8);
        }

        #region Commands

        protected AsyncRelayCommand AsyncCommand(Func<Task> execute) => new AsyncRelayCommand(execute);

        protected AsyncRelayCommand AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool updateWhenViewModelChanged = false)
        {
            return updateWhenViewModelChanged ? Associate(new AsyncRelayCommand(execute, canExecute)) : new AsyncRelayCommand(execute, canExecute);
        }

        protected AsyncRelayCommand<T> AsyncCommand<T>(Func<T, Task> execute) => new AsyncRelayCommand<T>(execute);

        protected AsyncRelayCommand<T> AsyncCommand<T>(Func<T, Task> execute, Predicate<T> canExecute, bool updateWhenViewModelChanged = false)
        {
            return updateWhenViewModelChanged ? Associate(new AsyncRelayCommand<T>(execute, canExecute)) : new AsyncRelayCommand<T>(execute, canExecute);
        }

        protected RelayCommand Command(Action execute) => new RelayCommand(execute);

        protected RelayCommand Command(Action execute, Func<bool> canExecute, bool updateWhenViewModelChanged = false)
        {
            return updateWhenViewModelChanged ? Associate(new RelayCommand(execute, canExecute)) : new RelayCommand(execute, canExecute);
        }

        private TCommand Associate<TCommand>(TCommand command) where TCommand : IRelayCommand
        {
            _commandMapping.Add(command);
            return command;
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            foreach (var command in _commandMapping)
            {
                command?.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 首次启动
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Start()
        {
        }
        
        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Start(Parameter arg)
        {
        }
        

        /// <summary>
        /// 表示关闭
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Stop()
        {
        }
        

        /// <summary>
        /// 表示挂起
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Suspend()
        {
        }

        /// <summary>
        /// 表示恢复
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
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