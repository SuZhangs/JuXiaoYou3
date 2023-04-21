namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        // TODO: 人物关系中的血缘关系
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            // AddSubView<CharacterInspirationView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation");
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                new PartOfAlbum { Items      = new List<Album>() },
                new PartOfPlaylist { Items   = new List<Music>() },
                new PartOfApprise { Items    = new List<Apprise>() },
                new PartOfStickyNote { Items = new List<StickyNote>() },
                new PartOfPrototype { Items  = new List<Prototype>() },
                new PartOfSentence { Items   = new List<Sentence>() },
                new PartOfSurvey { Items     = new List<SurveySet>() },
                new PartOfRel(),
            };
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum { Items    = new List<Album>() });
            document.Parts.Add(new PartOfPlaylist { Items = new List<Music>() });
            document.Parts.Add(new PartOfRel());
        }

        protected override void IsDataPartExistence(Document document)
        {
        }
    }
}