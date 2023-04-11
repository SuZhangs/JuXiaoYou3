using System.Windows;
using Acorisoft.FutureGL.MigaDB.Services;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class DocumentEditorViewModelProxy : BindingProxy<DocumentEditorVMBase>
    {
    }

    public abstract class DocumentEditorVMBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<SubViewBase> collection, string id) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name = Language.GetText(id),
                Type = typeof(TView)
            });
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

        protected sealed override void OnSubViewChanged(SubViewBase oldValue, SubViewBase newValue)
        {
            if (newValue is not HeaderedSubView subView)
            {
                return;
            }

            subView.Create(this);
            SubView = subView.SubView;
        }
    }
}