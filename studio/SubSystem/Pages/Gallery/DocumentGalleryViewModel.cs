
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

        public int Version { get; private set; }
        public DocumentEngine DocumentEngine { get; }
        public IDatabaseManager DatabaseManager { get; }
    }
}