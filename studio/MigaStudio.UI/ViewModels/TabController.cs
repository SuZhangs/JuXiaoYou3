using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabController : ViewModelBase, ITabViewController
    {
        private ITabViewModel _currentViewModel;

        protected TabController()
        {
            Workspace        = new ObservableCollection<ITabViewModel>();
            Outboards        = new ObservableCollection<ITabViewModel>();
            AddTabCommand    = new RelayCommand<object>(AddTabImpl);
            RemoveTabCommand = new RelayCommand<ITabViewModel>(RemoveTabImpl);
        }

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

        private void RemoveTabImpl(ITabViewModel viewModel)
        {
            RemoveTabItem(viewModel);
        }

        public void RemoveTabItem(ITabViewModel viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

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
                
                return;
            }
            Outboards.Remove(viewModel);
        }

        protected virtual void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
        }

        protected virtual void RequireStartupTabViewModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public TViewModel New<TViewModel>() where TViewModel : TabViewModel
        {
            var vm = Xaml.GetViewModel<TViewModel>();
            vm.Start(NavigationParameter.New(vm, this));
            Start(vm);
            return vm;
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

            if (string.IsNullOrEmpty(viewModel.Id))
            {
                viewModel.Start(NavigationParameter.New(viewModel, this).Params);
            }

            var unifiedKey = viewModel.Uniqueness ? viewModel.GetType().FullName : viewModel.Id;
            var result = true;

            foreach (var item in Workspace)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.Id;

                if (unifiedKey == unifiedKey2)
                {
                    CurrentViewModel = item;
                    result           = false;
                    break;
                }
            }

            foreach (var item in Outboards)
            {
                var unifiedKey2 = item.Uniqueness ? item.GetType().FullName : item.Id;

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
                        result           = false;
                    }

                    break;
                }
            }

            if (result)
            {
                if (Workspace.Count < MaximumWorkspaceItemCount)
                {
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

                if (value.Initialized)
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
        public RelayCommand<ITabViewModel> RemoveTabCommand { get; }
    }
}