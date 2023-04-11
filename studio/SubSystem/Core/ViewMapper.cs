using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public static class ViewMapper
    {
        private static readonly Dictionary<Type, Func<DocumentEditorBase, object, UserControl>> Mappings = new Dictionary<Type, Func<DocumentEditorBase,object, UserControl>>();

        public static void Add<T>(Func<DocumentEditorBase, object, UserControl> expression)
        {
            if (expression is null)
            {
                return;
            }

            Mappings.TryAdd(typeof(T), expression);
        }

        private static T Cast<T>(object instance) where T : class
        {
            return (T)instance;
        }

        private static UserControl GetAlbumView(DocumentEditorBase owner, object instance)
        {
            var vm = Cast<PartOfAlbum>(instance);
            return new AlbumPartView
            {
                DataContext = new AlbumPartViewModel
                {
                    Owner         = owner,
                    Detail        = vm,
                }
            }; 
        }
        
        private static UserControl GetPlaylistView(DocumentEditorBase owner, object instance)
        {
            var vm = Cast<PartOfPlaylist>(instance);
            return new PlaylistPartView
            {
                DataContext = new PlaylistPartViewModel
                {
                    Owner  = owner,
                    Detail = vm,
                }
            }; 
        }
        private static UserControl GetCharacterRel(DocumentEditorBase owner, object instance)
        {
            var vm = Cast<PartOfRel>(instance);
            return new CharacterRelshipPartView
            {
                DataContext = new CharacterRelPartViewModel
                {
                    Owner  = (DocumentEditorVMBase)owner,
                    Detail = vm,
                }
            }; 
        }
        
        private static UserControl GetDetailSetting(DocumentEditorBase owner, object instance)
        {
            return new PlaylistPartView
            {
                DataContext = owner
            }; 
        }
        
        public static void Initialize()
        {
            Add<PartOfAlbum>(GetAlbumView);
            Add<PartOfPlaylist>(GetPlaylistView);
            Add<PartOfRel>(GetCharacterRel);
            Add<DetailPartSettingPlaceHolder>(GetDetailSetting);
        }

        public static UserControl GetView(DocumentEditorBase owner, object viewModel)
        {
            if (viewModel is null)
            {
                return null;
            }

            if (Mappings.TryGetValue(viewModel.GetType(), out var mapping))
            {
                return mapping(owner, viewModel);
            }

            return new UserControl
            {
                DataContext = viewModel
            };
        }
    }
}