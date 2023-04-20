using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : TabController
    {
        private WindowState _windowState;

        public TabShell()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .PropertyTunnel
                .WindowState = x => WindowState = x;

            SelectInactiveWorkspaceCommand = AsyncCommand(SelectInactiveWorkspaceImpl, () => InactiveWorkspace.Count > 0);
        }

        protected override void OnAddViewModel(ITabViewModel viewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnAddViewModel(viewModel);
        }

        protected override void OnRemoveViewModel(ITabViewModel viewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnRemoveViewModel(viewModel);
        }

        protected override void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
            SelectInactiveWorkspaceCommand.NotifyCanExecuteChanged();
            base.OnCurrentViewModelChanged(oldViewModel, newViewModel);
        }

        private async Task SelectInactiveWorkspaceImpl()
        {
            if (InactiveWorkspace.Count == 0)
            {
                await SensitiveOperation(Language.GetText("text.noInactiveWorkspace"));
                return;
            }
            
            var r = await SubSystem.OptionSelection<TabViewModel>(
                SubSystemString.SelectTitle,
                InactiveWorkspace.FirstOrDefault(),
                InactiveWorkspace);

            if (!r.IsFinished)
            {
                return;
            }

            if (Workspace.Count <= 0)
            {
                return;
            }

            var lastActiveWorkspace     = Workspace[^1];
            var selectInactiveWorkspace = r.Value;

            var index = InactiveWorkspace.IndexOf(selectInactiveWorkspace);
            InactiveWorkspace[index] = lastActiveWorkspace;
            Workspace[^1]         = selectInactiveWorkspace;
            CurrentViewModel      = selectInactiveWorkspace;
        }
        
        protected override void StartOverride()
        {
            RequireStartupTabViewModel();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        public sealed override string Id => AppViewModel.IdOfTabShellController;

        /// <summary>
        /// 用于绑定的<see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }
        
        /// <summary>
        /// 选择
        /// </summary>
        public AsyncRelayCommand SelectInactiveWorkspaceCommand { get; }
    }
}