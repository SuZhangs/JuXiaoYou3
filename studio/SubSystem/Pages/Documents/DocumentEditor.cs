using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Share;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class DocumentEditorViewModelProxy : BindingProxy<DocumentEditorVMBase>
    {
    }

    public abstract class DocumentEditorVMBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id, string nc, string hc) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name           = Language.GetText(id),
                Type           = typeof(TView),
                DefaultColor   = nc,
                HighlightColor = hc
            });
        }

        protected static void AddBasicView<TView>(ICollection<HeaderedSubView> collection) where TView : FrameworkElement
        {
            AddSubView<TView>(collection, "text.DocumentEditor.Basic", "#0091A4", "#00AFC6");
        }

        protected static void AddPartView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
        }

        protected static void AddDetailView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail", "#A42300", "#CC2C00");
        }

        protected static void AddShareView(ICollection<HeaderedSubView> collection)
        {
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation", "#5700A4", "#6500BF");
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                CreateAlbum(),
                CreatePlaylist(),
                CreateStickyNote(),
                CreatePrototype(),
                CreateSurvey(),
            };
        }

        protected override void IsDataPartExistenceOverride(Document document)
        {
            HasDataPart<PartOfAlbum>(CreateAlbum, AddDataPartToDocument);
            HasDataPart<PartOfPlaylist>(CreatePlaylist, AddDataPartToDocument);
        }

        protected sealed override void OnSubViewChanged(HeaderedSubView oldValue, HeaderedSubView newValue)
        {
            if (newValue is null)
            {
                return;
            }

            newValue.Create(this);
            SubView = newValue.SubView;

            if (newValue.Type == typeof(ShareView))
            {
                Preshapes.ForEach(x => (x.DataContext as PreshapeViewModelBase)?.Resume());
                RefreshPresentation();
            }
        }

        public static PartOfAlbum CreateAlbum() => new PartOfAlbum { Items                = new List<Album>() };
        public static PartOfPlaylist CreatePlaylist() => new PartOfPlaylist { Items       = new List<Music>() };
        public static PartOfStickyNote CreateStickyNote() => new PartOfStickyNote { Items = new List<StickyNote>() };
        public static PartOfPrototype CreatePrototype() => new PartOfPrototype();
        public static PartOfSurvey CreateSurvey() => new PartOfSurvey { Items = new List<SurveySet>() };
        public static PartOfAppraise CreateAppraise() => new PartOfAppraise();
        public static PartOfSentence CreateSentence() => new PartOfSentence();
        public static PartOfRel CreateCharacterRelatives() => new PartOfRel();
    }
}