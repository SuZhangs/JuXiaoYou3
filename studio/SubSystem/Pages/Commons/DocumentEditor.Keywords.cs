using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private async Task AddKeywordImpl()
        {
            await DocumentUtilities.AddKeyword(
                Cache.Id,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                this.WarningNotification);
        }

        private async Task RemoveKeywordImpl(Keyword item)
        {
            await DocumentUtilities.RemoveKeyword(
                item, 
                Keywords, 
                KeywordEngine, 
                SetDirtyState, 
                this.Error);
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Keyword> RemoveKeywordCommand { get; }
    }
}