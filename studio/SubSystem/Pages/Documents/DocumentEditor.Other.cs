namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class OtherDocumentViewModel: DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<OtherBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Preview");
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                new PartOfAlbum { DataBags    = new Dictionary<string, string>() },
                new PartOfPlaylist { DataBags = new Dictionary<string, string>() },
            };
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{ DataBags    = new Dictionary<string, string>()});
            document.Parts.Add(new PartOfPlaylist{ DataBags = new Dictionary<string, string>()});
        }

        protected override void IsDataPartExistence(Document document)
        {
            
        }
    }
}