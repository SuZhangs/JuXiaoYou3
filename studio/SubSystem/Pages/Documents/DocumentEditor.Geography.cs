using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Basic;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class GeographyDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddBasicView<GeographyBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }
        
        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum { Items    = new List<Album>() });
            document.Parts.Add(new PartOfPlaylist { Items = new List<Music>() });
        }

        protected override void IsDataPartExistence(Document document)
        {
        }
    }
}