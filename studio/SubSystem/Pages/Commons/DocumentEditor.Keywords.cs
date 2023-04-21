using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private async Task AddKeywordImpl()
        {
            await DocumentUtilities.AddKeyword(Keywords,
                KeywordEngine,
                SetDirtyState,
                Warning);
        }

        private async Task RemoveKeywordImpl(string item)
        {
            await DocumentUtilities.RemoveKeyword(
                item, 
                Keywords, 
                KeywordEngine, 
                SetDirtyState, 
                DangerousOperation);
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}