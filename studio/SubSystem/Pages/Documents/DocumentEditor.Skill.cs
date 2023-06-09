namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class SkillDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddBasicView<SkillBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{Items = new List<Album>()});
        }

    }
}