using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        private ITabViewModel       _currentViewModel;
        private GlobalStudioContext _context;

        protected TabController()
        {
            Workspace        = new ObservableCollection<ITabViewModel>();
            Outboards        = new ObservableCollection<ITabViewModel>();
            AddTabCommand    = new RelayCommand<object>(AddTabImpl);
            RemoveTabCommand = new AsyncRelayCommand<ITabViewModel>(RemoveTabImpl);
            
        }

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">键盘参数</param>
        public void SetWindowEvent(WindowKeyEventArgs windowKeyEventArgs)
        {
            CurrentViewModel?.SetWindowEvent(windowKeyEventArgs);
        }

        /// <summary>
        /// 接收Windows参数
        /// </summary>
        /// <param name="windowKeyEventArgs">拖拽参数</param>
        public void SetWindowEvent(WindowDragDropArgs windowKeyEventArgs)
        {
            CurrentViewModel?.SetWindowEvent(windowKeyEventArgs);
        }

        #region AddTab / RemoveTab

        

        private void AddTabImpl(object param)
        {
            if (param is null)
            {
                RequireStartupTabViewModel();
                return;
            }

            if (param is Type vmType)
            {
                try
                {
                    var vm = Xaml.GetViewModel<ITabViewModel>(vmType);
                    Start(vm);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            if (param is ITabViewModel tab)
            {
                Start(tab);
            }
        }

        private async Task RemoveTabImpl(ITabViewModel viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

            if (!viewModel.Removable)
            {
                return;
            }

            if (viewModel.ApprovalRequired && 
                !await Xaml.Get<IBuiltinDialogService>().Danger(CloseTabItemCaption, CloseTabItemContent))
            {
                return;
            }
            
            RemoveTabWithPromise(viewModel);
        }

        internal void RemoveTabWithPromise(ITabViewModel viewModel)
        {
            var index = Workspace.IndexOf(viewModel);

            if (index > -1)
            {
                Workspace.RemoveAt(index);

                if (!ReferenceEquals(_currentViewModel, viewModel))
                {
                    return;
                }

                if (index < Workspace.Count)
                {
                    CurrentViewModel = Workspace[index];
                }
                else if (index == Workspace.Count && index != 0)
                {
                    if (Outboards.Count > 0)
                    {
                        var first = Outboards[0];
                        Outboards.RemoveAt(0);
                        Workspace.Add(first);
                        CurrentViewModel = first;
                    }
                    else
                    {
                        CurrentViewModel = Workspace[0];
                    }
                }
                else
                {
                    RequireStartupTabViewModel();
                }
                
                OnRemoveViewModel(viewModel);
                return;
            }
            
            OnRemoveViewModel(viewModel);
            Outboards.Remove(viewModel);
        }

        public Task RemoveTabItem(ITabViewModel viewModel)
        {
            return RemoveTabImpl(viewModel);
        }
        
        #endregion

        #region Start

        public sealed override void Start()
        {
            if (!IsInitialized)
            {
                StartOverride();
                IsInitialized = true;
            }
        }

        protected virtual void StartOverride()
        {
            
        }
        
        protected virtual void OnStart(RoutingEventArgs arg)
        {
            
        }

        protected sealed override void OnStartup(RoutingEventArgs arg)
        {
            _context = arg.Args[0] as GlobalStudioContext;
            OnStart(arg);
        }

        #endregion

        protected virtual void OnAddViewModel(ITabViewModel viewModel)
        {
            
        }
        
        protected virtual void OnRemoveViewModel(ITabViewModel viewModel)
        {
            
        }

        protected virtual void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
            if (newViewModel is null)
            {
                return;
            }
            
            if (string.IsNullOrEmpty(newViewModel.Title))
            {
                newViewModel.Title = Language.GetTypeName(newViewModel.GetType());
            }
        }

        protected virtual void RequireStartupTabViewModel()
        {
        }

        private bool DeterminedViewModelExists(string unifiedKey)
        {
            foreach (var item in Workspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    CurrentViewModel = item;
                    return true;
                }
            }

            foreach (var item in Outboards)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    if (Workspace.Count > 0)
                    {
                        var lastOneIndex = Workspace.Count - 1;
                        (Workspace[lastOneIndex], Outboards[0]) = (Outboards[0], Workspace[lastOneIndex]);
                        CurrentViewModel                        = Workspace[lastOneIndex];
                    }
                    else
                    {
                        CurrentViewModel = item;
                        return true;
                    }

                    break;
                }
            }

            return false;
        }
        
        private bool DeterminedViewModelExists(string unifiedKey, out ITabViewModel viewModel)
        {
            foreach (var item in Workspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    CurrentViewModel = item;
                    viewModel        = item;
                    return true;
                }
            }

            foreach (var item in Outboards)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.PageId;

                if (unifiedKey == unifiedKey2)
                {
                    if (Workspace.Count > 0)
                    {
                        var lastOneIndex = Workspace.Count - 1;
                        (Workspace[lastOneIndex], Outboards[0]) = (Outboards[0], Workspace[lastOneIndex]);
                        CurrentViewModel                        = Workspace[lastOneIndex];
                    }
                    else
                    {
                        CurrentViewModel = item;
                        viewModel        = item;
                        return true;
                    }

                    break;
                }
            }

            viewModel = null;
            return false;
        }
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="viewModel">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel New(Type viewModel) 
        {
            var vm = Xaml.GetViewModel<TabViewModel>(viewModel);
            vm.Startup(NavigationParameter.NewPage(vm, this).Params);
            Start(vm);
            return vm;
        }
        
        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="viewModel">指定要打开的视图模型类型</param>
        /// <param name="parameters">指定要打开的视图模型类型</param>
        /// <returns>返回一个新的实例。</returns>
        public TabViewModel Start(Type viewModel, object[] parameters) 
        {
            var vm = Xaml.GetViewModel<TabViewModel>(viewModel);
            vm.Startup(NavigationParameter.NewPage(vm, this, parameters).Params);
            return vm;
        }

        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <typeparam name="TViewModel">指定要打开的视图模型类型</typeparam>
        /// <returns>返回一个新的实例。</returns>
        public TViewModel New<TViewModel>() where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.NewPage(vm, this).Params);
            Start(vm);
            return vm;
        }
        
        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="cache">索引</param>
        /// <returns>返回一个新的导航参数。</returns>
        public TViewModel OpenDocument<TViewModel>(IDataCache cache)where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Startup(NavigationParameter.OpenDocument(cache, this).Params);
            Start(vm);
            return vm;
        }

        /// <summary>
        /// 打开视图模型
        /// </summary>
        /// <param name="id">唯一标识符</param>
        /// <typeparam name="TViewModel">指定要打开的视图模型类型</typeparam>
        /// <returns>返回一个新的实例或已经打开的视图模型。</returns>
        public TViewModel New<TViewModel>(string id) where TViewModel : TabViewModel
        {
            if (DeterminedViewModelExists(id, out var vm))
            {
                return (TViewModel)vm;
            }
            
            var vm1 = Xaml.GetViewModel<TViewModel>();
            vm1.Startup(NavigationParameter.NewPage(vm, this).Params);
            Start(vm1);
            return vm1;
        }
        
        /// <summary>
        /// 启动指定的视图模型
        /// </summary>
        /// <param name="viewModel">指定的视图模型</param>
        public void Start(ITabViewModel viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(viewModel.PageId))
            {
                viewModel.Startup(NavigationParameter.NewPage(viewModel, this).Params);
            }

            var unifiedKey = viewModel.Uniqueness ? viewModel.GetType().FullName : viewModel.PageId;
            var result = !DeterminedViewModelExists(unifiedKey);

            if (result)
            {
                if (Workspace.Count < MaximumWorkspaceItemCount)
                {
                    OnAddViewModel(viewModel);
                    Workspace.Add(viewModel);
                    CurrentViewModel = viewModel;
                }
                else
                {
                    // TODO: 添加
                    Xaml.Get<INotifyService>().Notify(new IconNotification
                    {
                        Title = "已经达到最高上限了！"
                    });
                }
            }
        }

        /// <summary>
        /// 最大工作区项目数
        /// </summary>
        public int MaximumWorkspaceItemCount => (int)((SystemParameters.WorkArea.Width - 300) / 100);

        /// <summary>
        /// 当前的视图模型
        /// </summary>
        public ITabViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Suspend();

                if (value is null)
                {
                    return;
                }

                if (value.IsInitialized)
                {
                    value.Resume();
                }
                else
                {
                    value.Start();
                }

                OnCurrentViewModelChanged(_currentViewModel, value);
                SetValue(ref _currentViewModel, value);
            }
        }
        
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// 工作区标签页
        /// </summary>
        public ObservableCollection<ITabViewModel> Workspace { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ITabViewModel> Outboards { get; }

        /// <summary>
        /// 添加标签页
        /// </summary>
        public RelayCommand<object> AddTabCommand { get; }

        /// <summary>
        /// 移除标签页
        /// </summary>
        public AsyncRelayCommand<ITabViewModel> RemoveTabCommand { get; }


        public GlobalStudioContext Context => _context;

        internal static string CloseTabItemCaption
        {
            get
            {
                // TODO: 翻译
                return Language.Culture switch
                {
                    _ => "确认退出？"
                };
            }
        }
        
        internal static string CloseTabItemContent
        {
            get
            {
                // TODO: 翻译
                return Language.Culture switch
                {
                    _ => "当前页面尚未保存，您确定要退出？"
                };
            }
        }
    }
}