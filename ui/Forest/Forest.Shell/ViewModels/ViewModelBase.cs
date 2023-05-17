using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
namespace Acorisoft.FutureGL.Forest.ViewModels
{
    /// <summary>
    /// <see cref="ViewModelBase"/> 表示一个视图模型基类。
    /// </summary>
    public abstract class ViewModelBase : ForestObject, IViewModel, IViewModelLanguageService
    {
        private readonly        List<IRelayCommand> _commandMapping;
        private                 string              _rootName;
        private                 bool                _initialized;

        protected ViewModelBase()
        {
            _commandMapping = new List<IRelayCommand>(8);
            Collector       = new DisposableCollector(8);
        }


        protected static bool HasItem<T>(T item) where T : class
            => item is not null;
        protected static bool NotLastItem<T>(IList<T> collection, T item)  where T : class
            => item is not null && collection.IndexOf(item) < collection.Count - 1;
        protected static bool NotFirstItem<T>(IList<T> collection, T item)  where T : class
            => item is not null && collection.IndexOf(item) > 0;

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
        
        protected RelayCommand<T> Command<T>(Action<T> execute) => new RelayCommand<T>(execute);

        protected RelayCommand Command(Action execute, Func<bool> canExecute, bool updateWhenViewModelChanged = false)
        {
            return updateWhenViewModelChanged ? Associate(new RelayCommand(execute, canExecute)) : new RelayCommand(execute, canExecute);
        }
        
        protected RelayCommand<T> Command<T>(Action<T> execute, Predicate<T> canExecute, bool updateWhenViewModelChanged = false)
        {
            return updateWhenViewModelChanged ? Associate(new RelayCommand<T>(execute, canExecute)) : new RelayCommand<T>(execute, canExecute);
        }

        private TCommand Associate<TCommand>(TCommand command) where TCommand : IRelayCommand
        {
            _commandMapping.Add(command);
            return command;
        }

        #endregion

        protected static IDialogService DialogService() => Xaml.Get<IDialogService>();

        protected sealed override void ReleaseManagedResources()
        {
            _commandMapping.Clear();
            Collector.Dispose();
            ReleaseManagedResourcesOverride();
        }

        protected virtual void ReleaseManagedResourcesOverride()
        {
            
        }

        protected sealed override void OnPropertyValueChanged(string propertyName)
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
        
        [NullCheck(UniTestLifetime.Startup)]
        public RoutingEventArgs OriginalParameter { get; set; }
        
        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Startup(RoutingEventArgs arg)
        {
            if (_initialized)
            {
                return;
            }

            _initialized      = true;
            OriginalParameter = arg;
            OnStartup(arg);
        }


        protected virtual void OnStartup(RoutingEventArgs arg)
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
        
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInitialized { get; protected set; }

        string IViewModelLanguageService.RootName
        {
            get => _rootName;
            set => _rootName = value;
        }
    }
}