using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private async Task AddKeywordImpl()
        {
            if (Keywords.Count >= 32)
            {
                await Warning(SubSystemString.KeywordTooMany);
            }

            var hash = Keywords.ToHashSet();
            var r    = await StringViewModel.String(SubSystemString.AddKeywordTitle);

            if (!r.IsFinished)
            {
                return;
            }

            if (!hash.Add(r.Value))
            {
                await Warning(Language.ContentDuplicatedText);
                return;
            }

            KeywordEngine.AddKeyword(r.Value);
            Keywords.Add(r.Value);
            SetDirtyState();
        }

        private async Task RemoveKeywordImpl(string item)
        {
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (!Keywords.Remove(item))
            {
                return;
            }

            Keywords.Remove(item);
            SetDirtyState();
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}