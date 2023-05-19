using System.Diagnostics;
using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : ShellCore
    {
        public TabShell()
        {
            SelectInactiveWorkspaceCommand = AsyncCommand(SelectInactiveWorkspaceImpl);
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
                await this.WarningNotification(Language.GetText("text.noInactiveWorkspace"));
                return;
            }

            var r = await SubSystem.Selection<TabViewModel>(
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
            Workspace[^1]            = selectInactiveWorkspace;
            CurrentViewModel         = selectInactiveWorkspace;
        }

        protected override void StartOverride()
        {
            RequireStartupTabViewModel();
#if DEBUG
            New<WeaponEditorViewModel>();
            New<PlanetEditorViewModel>();
            New<MaterialEditorViewModel>();
            New<UniverseEditorViewModel>();
            New<KeywordViewModel>();
#endif
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        public sealed override string Id => AppViewModel.IdOfTabShellController;

        /// <summary>
        /// 选择
        /// </summary>
        public AsyncRelayCommand SelectInactiveWorkspaceCommand { get; }
    }
}