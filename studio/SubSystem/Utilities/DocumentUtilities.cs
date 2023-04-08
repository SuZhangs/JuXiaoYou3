using System.IO;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class DocumentUtilities
    {
        public static async Task AddDocument(DocumentEngine engine, Action<DocumentCache> callback)
        {
            var result = await NewDocumentViewModel.New();

            if (!result.IsFinished)
            {
                return;
            }

            var cache  = result.Value;
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
        
        
        public static async Task EditDocument(DocumentEngine engine, ImageEngine imageEngine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null)
            {
                return;
            }

            
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
                engine.UpdateDocument(document,cache);
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