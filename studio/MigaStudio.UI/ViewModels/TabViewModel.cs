using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    [DebuggerDisplay("{Id}")]
    public abstract class TabViewModel : PageViewModel, IEquatable<TabViewModel>, ITabViewModel
    {
        private string _title;
        private bool   _isPinned;
        private bool _initialized;
        
        #region Override

        

        public bool Equals(TabViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Uniqueness ?
                other.GetType() == GetType() :
                Id == other.Id;
        }

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TabViewModel)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region Start / OnStart

        
        public sealed override void Start()
        {
            try
            {
                OnStart();
                _initialized = true;
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override void Start(Parameter arg)
        {
            var np = NavigationParameter.FromParameter(arg);
            Id         = np.Id;
            Controller = (TabController)np.Controller;
            OnStart(np);
        }
        
        public void Start(NavigationParameter arg)
        {
            Id = arg.Id;
            OnStart(arg);
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
            vm.Start(NavigationParameter.New(vm, Controller));
            Controller.Start(vm);
            return vm;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        /// <typeparam name="TViewModel"></typeparam>
        public void New<TViewModel>(IData data, IDataCache cache) where TViewModel : TabViewModel
        {
            var vm = Xaml.Get<TViewModel>();
            vm.Start(NavigationParameter.New(data, cache, Controller));
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
        public string Id { get; private set; }
        
        
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