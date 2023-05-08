namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class AbilityDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddBasicView<AbilityBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{Items = new List<Album>()});
        }

        protected override void IsDataPartExistence(Document document)
        {
            
        }
    }
}