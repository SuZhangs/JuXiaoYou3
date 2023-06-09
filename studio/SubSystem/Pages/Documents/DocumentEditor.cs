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
                Name = Language.GetText(id),
                Type = typeof(TView),
                DefaultColor = nc,
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
                new PartOfAlbum { Items      = new List<Album>() },
                new PartOfPlaylist { Items   = new List<Music>() },
                new PartOfStickyNote { Items = new List<StickyNote>() },
                new PartOfPrototype(),
                new PartOfSurvey { Items     = new List<SurveySet>() },
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected sealed override void OnSubViewChanged(HeaderedSubView oldValue, HeaderedSubView newValue)
        {
            if (newValue is not HeaderedSubView subView)
            {
                return;
            }

            subView.Create(this);
            SubView = subView.SubView;

            if (subView.Type == typeof(ShareView))
            {
                Preshapes.ForEach(x => (x.DataContext as PreshapeViewModelBase)?.Resume());
                RefreshPresentation();
            }
        }
    }
}