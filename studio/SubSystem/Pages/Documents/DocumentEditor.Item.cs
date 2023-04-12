
namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class ItemDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<ItemBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            //AddSubView<InspirationView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation");
        }
        
        
        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{ Items     = new List<Album>()});
        }

        protected override void IsDataPartExistence(Document document)
        {
            
        }
    }
}