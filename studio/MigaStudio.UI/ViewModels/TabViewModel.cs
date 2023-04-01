using System.Diagnostics;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
// ReSharper disable StringLiteralTypo

// ReSharper disable MemberCanBeMadeStatic.Global

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    [DebuggerDisplay("{PageId}-{Title}")]
    public abstract class TabViewModel : PageViewModel, IEquatable<TabViewModel>, ITabViewModel
    {
        private string _title;
        private bool   _isPinned;
        private bool   _initialized;

        private static readonly GeometryGroup Checked;
        private static readonly Geometry      InfoGeometry;
        private static readonly Geometry      ErrorGeometry;

        static TabViewModel()
        {
            InfoGeometry = Geometry.Parse("F1 M24,24z M0,0z M21,15A2,2,0,0,1,19,17L7,17 3,21 3,5A2,2,0,0,1,5,3L19,3A2,2,0,0,1,21,5z");
            ErrorGeometry = new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry{ StartPoint = new Point(18,6), EndPoint = new Point(6,18)},
                    new LineGeometry{ StartPoint = new Point(6,6), EndPoint = new Point(18,18)},
                    new EllipseGeometry{ Center = new Point(9,9), RadiusX = 9, RadiusY = 9},
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

        protected TabViewModel()
        {
            ApprovalRequired = true;
        }

        private static string Unsave
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "[+] {0}",
                CultureArea.French   => "[+] {0}",
                CultureArea.Japanese => "[+] {0}",
                CultureArea.Korean   => "[+] {0}",
                CultureArea.Russian  => "[+] {0}",
                _                    => "[+] {0}",
            };
        }

        protected void SetTitle(string title, bool useUnsavePattern = false)
        {
            Title = useUnsavePattern ? string.Format(Unsave, title) : title;
        }

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

        protected Task<bool> DangerousOperation(string content) => Xaml.Get<IDialogService>()
                                                                       .Danger(DangerOperationCaption, content);

        protected Task<bool> SensitiveOperation(string content) => Xaml.Get<IDialogService>()
                                                                       .Danger(SensitiveOperationCaption, content);


        protected Task Obsoleted(string content) => Xaml.Get<IDialogService>()
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

        protected Task Warning(string content) => Xaml.Get<IDialogService>()
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

        #region Override

        public bool Equals(TabViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Uniqueness ? other.GetType() == GetType() : PageId == other.PageId;
        }

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TabViewModel)obj);
        }

        public override int GetHashCode()
        {
            return PageId?.GetHashCode() ?? base.GetHashCode();
        }

        #endregion

        #region Start / OnStart

        public sealed override void Start()
        {
            try
            {
                if (!_initialized)
                {
                    OnStart();
                    _initialized = true;
                }
            }
            catch
            {
                _initialized = false;
            }
        }

        public virtual void OnStart()
        {
        }

        /// <summary>
        /// 传递参数。
        /// </summary>
        /// <param name="arg"></param>
        protected sealed override void OnStartup(RouteEventArgs arg)
        {
            var np = NavigationParameter.FromParameter(arg);
            PageId     = np.Id;
            Controller = (TabController)np.Controller;
            OnStart(np);
        }

        protected virtual void OnStart(NavigationParameter parameter)
        {
        }

        #endregion

        #region Controller

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public TViewModel New<TViewModel>() where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(vm, Controller).Params);
            Controller.Start(vm);
            return vm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <typeparam name="TViewModel"></typeparam>
        public void New<TViewModel>(IDataCache cache) where TViewModel : TabViewModel
        {
            var vm = Xaml.Get<TViewModel>();
            vm.Startup(NavigationParameter.OpenDocument(cache, Controller).Params);
            Controller.Start(vm);
        }

        #endregion

        protected TabController Controller { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// 用于表示当前的视图模型的唯一标识符。
        /// </summary>
        public string PageId { get; private set; }


        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (_isPinned != value)
                {
                    SetValue(ref _isPinned, value);
                }
            }
        }

        /// <summary>
        /// 是否可被关闭
        /// </summary>
        public virtual bool Removable => true;

        /// <summary>
        /// 需要询问
        /// </summary>
        public bool ApprovalRequired { get; protected set; }

        /// <summary>
        /// 用来表示当前的视图模型是否为唯一的。
        /// </summary>
        /// <remarks>
        /// <see cref="Uniqueness"/> 属性用来表示是否唯一，这个唯一是按照类型来算的。如果这个值为true，那么只能存在一个打开的类型。
        /// </remarks>
        public virtual bool Uniqueness => false;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public bool Initialized => _initialized;
    }
}