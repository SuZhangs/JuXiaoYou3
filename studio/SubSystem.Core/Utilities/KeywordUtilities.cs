using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.IO;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class KeywordUtilities
    {
        public static async Task AddKeyword(
            string documentID,
            IList<Keyword> keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task> Warning)
        {
            if (KeywordEngine.GetKeywordCount(documentID) >= 32)
            {
                await Warning(SubSystemString.KeywordTooMany);
            }

            var r    = await SingleLineViewModel.String(SubSystemString.AddKeywordTitle);

            if (!r.IsFinished)
            {
                return;
            }

            if (KeywordEngine.HasKeyword(documentID, r.Value))
            {
                await Warning(Language.ContentDuplicatedText);
                return;
            }

            var key = new Keyword
            {
                Id         = ID.Get(),
                DocumentId = documentID,
                Name       = r.Value
            };
            keywords.Add(key);
            KeywordEngine.AddKeyword(key);
            SetDirtyState(true);
        }

        public static async Task RemoveKeyword(
            Keyword item,
            IList<Keyword> keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task<bool>> DangerousOperation)
        {
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (!keywords.Remove(item))
            {
                return;
            }

            keywords.Remove(item);
            KeywordEngine.RemoveKeyword(item);
            SetDirtyState(true);
        }
    }
}