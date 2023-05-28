using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class DocumentEditorViewModelProxy : BindingProxy<DocumentEditorVMBase>
    {
    }

    public abstract class DocumentEditorVMBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<SubViewBase> collection, string id, string nc, string hc) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name = Language.GetText(id),
                Type = typeof(TView),
                DefaultColor = nc,
                HighlightColor = hc
            });
        }

        protected static void AddBasicView<TView>(ICollection<SubViewBase> collection) where TView : FrameworkElement
        {
            AddSubView<TView>(collection, "text.DocumentEditor.Basic", "#0091A4", "#00AFC6");
        }

        protected static void AddPartView(ICollection<SubViewBase> collection)
        {
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
        }
        
        protected static void AddDetailView(ICollection<SubViewBase> collection)
        {
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail", "#A42300", "#CC2C00");
        }
        
        protected static void AddShareView(ICollection<SubViewBase> collection)
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
                new PartOfPrototype { Items  = new List<Prototype>() },
                new PartOfSurvey { Items     = new List<SurveySet>() },
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected sealed override void OnSubViewChanged(SubViewBase oldValue, SubViewBase newValue)
        {
            if (newValue is not HeaderedSubView subView)
            {
                return;
            }

            subView.Create(this);
            SubView = subView.SubView;

            if (subView.Type == typeof(ShareView))
            {
                RefreshPresentation();
            }
        }
    }
}