
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class DocumentGalleryViewModel : TabViewModel
    {
        public DocumentGalleryViewModel()
        {
            DatabaseManager = Xaml.Get<IDatabaseManager>();
            DocumentEngine  = DatabaseManager.GetEngine<DocumentEngine>();
        }

        public void Refresh()
        {
            
        }

        public override void OnStart()
        {
            Version = DocumentEngine.Version;
            base.OnStart();
        }

        public override void Resume()
        {
            if (Version != DocumentEngine.Version)
            {
                Version = DocumentEngine.Version;
                Refresh();
            }
            
            base.Resume();
        }

        /// <summary>
        /// 当前的引擎版本，用于判断是否重新加载内容。
        /// </summary>
        /// <remarks>重新加载内容，这个过程虽然对于后端是没有什么压力的，但是对于前端以及大量的IO操作则是致命性的。</remarks>
        public int Version { get; private set; }
        
        
        public DocumentEngine DocumentEngine { get; }
        
        public IDatabaseManager DatabaseManager { get; }
        
        
    }
}