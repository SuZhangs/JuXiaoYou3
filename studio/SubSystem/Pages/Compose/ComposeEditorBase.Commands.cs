using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public partial class ComposeEditorBase
    {
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveComposeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewComposeCommand { get; }
    }
}