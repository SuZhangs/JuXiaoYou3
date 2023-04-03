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
        private static readonly GeometryGroup       Checked;
        private static readonly Geometry            InfoGeometry;
        private static readonly Geometry            ErrorGeometry;
        private readonly        List<IRelayCommand> _commandMapping;
        private                 string              _rootName;
        private                 bool                _initialized;
        
        
        static ViewModelBase()
        {
            InfoGeometry = Geometry.Parse("F1 M24,24z M0,0z M21,15A2,2,0,0,1,19,17L7,17 3,21 3,5A2,2,0,0,1,5,3L19,3A2,2,0,0,1,21,5z");
            ErrorGeometry = new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry{ StartPoint = new Point(18,6), EndPoint = new Point(6,18)},
                    new LineGeometry{ StartPoint = new Point(6,6), EndPoint  = new Point(18,18)},
                    new EllipseGeometry{ Center  = new Point(9,9), RadiusX   = 9, RadiusY = 9},
                }
            };
            Checked = new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry
                    {
                        StartPoint = new Point(12, 5),
                        EndPoint   = new Point(12, 19)
                    },
                    Geometry.Parse("F1 M24,24z M0,0z M19,12L19,12 12,19 5,12")
                }
            };
        }


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
        
        #region Notify

        static string DangerOperationCaption => Language.Culture switch
        {
            CultureArea.English  => "Dangerous Operation",
            CultureArea.Korean   => "위험한 조작",
            CultureArea.Japanese => "危険な操作です",
            CultureArea.French   => "Une opération dangereuse",
            CultureArea.Russian  => "Опасная операция",
            _                    => "危险操作"
        };

        static string SensitiveOperationCaption => Language.Culture switch
        {
            CultureArea.English  => "Sensitive operation",
            CultureArea.Korean   => "민감한 조작",
            CultureArea.Japanese => "デリケートな操作です",
            CultureArea.French   => "Une opération sensible",
            CultureArea.Russian  => "Деликатная операция",
            _                    => "敏感操作"
        };

        static string ErrorCaption => Language.Culture switch
        {
            CultureArea.English  => "Error",
            CultureArea.Korean   => "오류",
            CultureArea.Japanese => "間違いです",
            CultureArea.French   => "Les erreurs",
            CultureArea.Russian  => "ошибк",
            _                    => "错误"
        };

        static string WarningCaption => Language.Culture switch
        {
            CultureArea.English  => "Warning",
            CultureArea.Korean   => "경고",
            CultureArea.Japanese => "警告します",
            CultureArea.French   => "Avertissement",
            CultureArea.Russian  => "предупред",
            _                    => "警告"
        };

        static string InfoCaption => Language.Culture switch
        {
            CultureArea.English  => "Error",
            CultureArea.Korean   => "오류",
            CultureArea.Japanese => "間違いです",
            CultureArea.French   => "Les erreurs",
            CultureArea.Russian  => "ошибк",
            _                    => "错误"
        };

        static string SuccessfulCaption => Language.Culture switch
        {
            CultureArea.English  => "Successful",
            CultureArea.Korean   => "성공",
            CultureArea.Japanese => "成功です",
            CultureArea.French   => "Le succès",
            CultureArea.Russian  => "успешн",
            _                    => "成功"
        };

        static string ObsoletedCaption => Language.Culture switch
        {
            CultureArea.English  => "Obsoleted",
            CultureArea.Korean   => "기한이 지나면",
            CultureArea.Japanese => "期限切れです",
            CultureArea.French   => "Périmés",
            CultureArea.Russian  => "Срок годност",
            _                    => "过期"
        };

        protected Task<bool> DangerousOperation(string content) => DialogService()
                                                                       .Danger(DangerOperationCaption, content);

        protected Task<bool> SensitiveOperation(string content) => DialogService()
                                                                       .Danger(SensitiveOperationCaption, content);


        protected Task Obsoleted(string content) => DialogService()
                                                        .Notify(CriticalLevel.Obsoleted, ObsoletedCaption, content);


        protected void Successful(string content, int seconds = 2) => Xaml.Get<INotifyService>()
                                                                          .Notify(new IconNotification
                                                                          {
                                                                              Color = ThemeSystem.Instance
                                                                                                 .Theme
                                                                                                 .Colors[(int)ForestTheme.Success100],
                                                                              Delay    = TimeSpan.FromSeconds(seconds),
                                                                              Geometry = Checked,
                                                                              IsFilled = false,
                                                                              Title    = content
                                                                          });

        protected Task Warning(string content) => DialogService()
                                                      .Notify(CriticalLevel.Warning, WarningCaption, content);
        
        protected void Info(string content, int seconds = 2) => Xaml.Get<INotifyService>()
                                                   .Notify(new IconNotification
                                                   {
                                                       Color = ThemeSystem.Instance
                                                                          .Theme
                                                                          .Colors[(int)ForestTheme.Info100],
                                                       Delay    = TimeSpan.FromSeconds(seconds),
                                                       Geometry = InfoGeometry,
                                                       IsFilled = false,
                                                       Title    = content
                                                   });
        
        
        protected void Error(string content, int seconds = 2) => Xaml.Get<INotifyService>()
                                                                     .Notify(new IconNotification
                                                                     {
                                                                         Color = ThemeSystem.Instance
                                                                                            .Theme
                                                                                            .Colors[(int)ForestTheme.Danger100],
                                                                         Delay    = TimeSpan.FromSeconds(seconds),
                                                                         Geometry = ErrorGeometry,
                                                                         IsFilled = false,
                                                                         Title    = content
                                                                     });

        #endregion

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
        
        [NullCheck(UniTestLifetime.Startup)]
        public RouteEventArgs OriginalParameter { get; set; }
        
        /// <summary>
        /// 表示参数传递。
        /// </summary>
        /// <param name="arg">视图参数</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Startup(RouteEventArgs arg)
        {
            if (_initialized)
            {
#if DEBUG
                throw new InvalidOperationException();
#else
                return;
#endif
            }

            _initialized      = true;
            OriginalParameter = arg;
            OnStartup(arg);
        }


        protected virtual void OnStartup(RouteEventArgs arg)
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