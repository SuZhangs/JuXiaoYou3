using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using CommunityToolkit.Mvvm.Input;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class DocumentUtilities
    {
        public static async Task AddKeyword(
            IList<string> Keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task> Warning)
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
            SetDirtyState(true);
        }

        public static async Task RemoveKeyword(
            string item,
            IList<string> Keywords,
            KeywordEngine KeywordEngine,
            Action<bool> SetDirtyState,
            Func<string, Task<bool>> DangerousOperation)
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
            KeywordEngine.RemoveKeyword(item);
            SetDirtyState(true);
        }

        public static async Task AddDocument(DocumentEngine engine, Action<DocumentCache> callback)
        {
            var result = await NewDocumentViewModel.New();

            if (!result.IsFinished)
            {
                return;
            }

            var cache   = result.Value;
            var result1 = engine.AddDocumentCache(cache);

            if (!result.IsFinished)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Warning,
                              SubSystemString.Notify,
                              SubSystemString.GetEngineResult(result1.Reason));
            }

            callback?.Invoke(cache);
        }

        public static async Task AddDocument(DocumentEngine engine, DocumentType type, Action<DocumentCache> callback)
        {
            var result = await NewDocumentViewModel.New(type);

            if (!result.IsFinished)
            {
                return;
            }

            var cache   = result.Value;
            var result1 = engine.AddDocumentCache(cache);

            if (!result.IsFinished)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Warning,
                              SubSystemString.Notify,
                              SubSystemString.GetEngineResult(result1.Reason));
            }

            callback?.Invoke(cache);
        }

        public static DocumentCache UpdateDocument(DocumentEngine engine, string id, IList<DocumentCache> sourceA, IList<DocumentCache> sourceB)
        {
            if (sourceA is null ||
                sourceB is null ||
                engine is null ||
                string.IsNullOrEmpty(id))
            {
                return null;
            }

            var indexA = sourceA.IndexOf(x => x.Id == id);
            var indexB = sourceB.IndexOf(x => x.Id == id);

            var inside = engine.DocumentCacheDB.FindById(id);

            if (inside is null)
            {
                return null;
            }

            sourceA[indexA] = inside;
            sourceB[indexB] = inside;
            return inside;
        }

        public static void OpenDocument(TabController controller, DocumentCache cache)
        {
            if (cache is null || controller is null)
            {
                return;
            }

            switch (cache.Type)
            {
                case DocumentType.None:
                    break;
                case DocumentType.Ability:
                    controller.OpenDocument<AbilityDocumentViewModel>(cache);
                    break;
                case DocumentType.Character:
                    controller.OpenDocument<CharacterDocumentViewModel>(cache);
                    break;
                case DocumentType.Item:
                    controller.OpenDocument<ItemDocumentViewModel>(cache);
                    break;
                case DocumentType.Geography:
                    controller.OpenDocument<GeographyDocumentViewModel>(cache);
                    break;
                case DocumentType.Other:
                    controller.OpenDocument<OtherDocumentViewModel>(cache);
                    break;
                case DocumentType.Document:
                    break;
                case DocumentType.Universe:
                    break;
                case DocumentType.World:
                    break;
                case DocumentType.God:
                    break;
                case DocumentType.Devil:
                    break;
                case DocumentType.NPC:
                    break;
                case DocumentType.Compose:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static async Task ChangedDocument(DocumentEngine engine, ImageEngine imageEngine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null ||
                engine is null ||
                imageEngine is null)
            {
                return;
            }

            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (imageEngine.HasFile(md5))
            {
                var fr = imageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = ImageUtilities.GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                imageEngine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                imageEngine.AddFile(record);
            }

            cache.Avatar = avatar;
            SyncDocument(engine, cache);
        }

        public static void SyncDocument(DocumentEngine engine, DocumentCache cache)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            var document = engine.DocumentDB.FindById(cache.Id);

            if (document is null)
            {
                engine.UpdateDocument(cache);
            }
            else
            {
                document.Name   = cache.Name;
                document.Avatar = cache.Avatar;
                document.Intro  = cache.Intro;
                engine.UpdateDocument(document, cache);
            }
        }

        public static void RemoveDocument(DocumentEngine engine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            engine.RemoveDocumentCache(cache);
            callback?.Invoke(cache);
        }

        public static void LockOrUnlock(DocumentEngine engine, DocumentCache cache)
        {
            if (cache is null || engine is null)
            {
                return;
            }

            cache.IsLocked = !cache.IsDeleted;
            engine.UpdateDocument(cache);
        }
    }
}