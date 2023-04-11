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
                new PartOfAlbum { Items    = new List<Album>() },
                new PartOfPlaylist { Items = new List<Music>() },
                new PartOfApprise{ },
                new PartOfRel()
            };
        }

        protected sealed override FrameworkElement CreateDetailPartView(IPartOfDetail part)
        {
            if (part is PartOfAlbum album)
            {
                return new AlbumPartView
                {
                    DataContext = new AlbumPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = album,
                        Document        = Document,
                        DocumentCache   = Cache,
                    }
                };
            }

            if (part is PartOfPlaylist playlist)
            {
                return new PlaylistPartView
                {
                    DataContext = new PlaylistPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = playlist,
                        Document        = Document,
                        DocumentCache   = Cache,
                    }
                };
            }

            if (part is PartOfRel rel)
            {
                return new CharacterRelshipPartView
                {
                    DataContext = new CharacterRelPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = rel,
                        Document        = Document,
                        DocumentCache   = Cache,
                    }
                };
            }

            if (part is DetailPartSettingPlaceHolder)
            {
                return new DetailPartSettingView
                {
                    DataContext = this
                };
            }

            return null;
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