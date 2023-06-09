using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class KeywordEditable<TCache, TDocument> : DocumentEditable<TCache, TDocument>
        where TDocument : class, IData
        where TCache : class, IDataCache
    {
        protected KeywordEditable()
        {
            Keywords      = new ObservableCollection<Keyword>();
            KeywordEngine = Studio.Engine<KeywordEngine>();

            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<Keyword>(RemoveKeywordImpl, x => x is not null);
        }

        protected void SynchronizeKeywords()
        {
            Keywords.AddMany(KeywordEngine.GetKeywords(Cache.Id), true);
        }

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

        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<Keyword> Keywords { get; }
    }
}