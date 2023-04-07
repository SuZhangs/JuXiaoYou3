using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public class DocumentUtilities
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

        public static void RemoveDocument(DocumentEngine engine, DocumentCache cache, Action<DocumentCache> callback)
        {
            if (cache is null)
            {
                return;
            }
            
            engine.RemoveDocumentCache(cache);
            callback?.Invoke(cache);
        }
    }
}