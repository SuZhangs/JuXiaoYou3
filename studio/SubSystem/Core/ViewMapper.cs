using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
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

        private static UserControl GetAlbumView(DocumentEditorBase owner, object instance)
        {
            return new AlbumPartView
            {
                DataContext = new AlbumPartViewModel
                {
                    Owner         = owner,
                    Detail        =  (PartOfAlbum)instance,
                }
            };
        }
        
        private static UserControl GetPlaylistView(DocumentEditorBase owner, object instance)
        {
            return new PlaylistPartView
            {
                DataContext = new PlaylistPartViewModel
                {
                    Owner  = owner,
                    Detail =  (PartOfPlaylist)instance,
                }
            }; 
        }
        private static UserControl GetCharacterRel(DocumentEditorBase owner, object instance)
        {
            return new CharacterRelshipPartView
            {
                DataContext = new CharacterRelPartViewModel
                {
                    Owner  = (DocumentEditorVMBase)owner,
                    Detail =  (PartOfRel)instance,
                }
            }; 
        }
        
        private static UserControl GetSurvey(DocumentEditorBase owner, object instance)
        {
            return new SurveyPartView
            {
                DataContext = new SurveyPartViewModel
                {
                    Owner  = owner,
                    Detail =  (PartOfSurvey)instance,
                }
            }; 
        }
        
        private static UserControl GetStickyNote(DocumentEditorBase owner, object instance)
        {
            return new StickyNotePartView
            {
                DataContext = new StickyNotePartViewModel
                {
                    Owner  = owner,
                    Detail =  (PartOfStickyNote)instance,
                }
            }; 
        }
        
        private static UserControl GetSentenceView(DocumentEditorBase owner, object instance)
        {
            return new SentencePartView
            {
                DataContext = new SentencePartViewModel
                {
                    Owner  = owner,
                    Detail =  (PartOfSentence)instance,
                }
            }; 
        }
        
        private static UserControl GetPrototypeView(DocumentEditorBase owner, object instance)
        {
            return new PrototypePartView
            {
                DataContext = new PrototypePartViewModel
                {
                    Owner = owner,
                    Detail = (PartOfPrototype)instance
                }
            }; 
        }
        
        private static UserControl GetAppriseView(DocumentEditorBase owner, object instance)
        {
            return new ApprisePartView
            {
                DataContext = new ApprisePartViewModel
                {
                    Owner = owner,
                    Detail = (PartOfApprise)instance
                }
            }; 
        }
        
        private static UserControl GetDetailSetting(DocumentEditorBase owner, object instance)
        {
            return new DetailPartSettingView
            {
                DataContext = owner
            }; 
        }
        
        
        
        public static void Initialize()
        {
            Add<PartOfAlbum>(GetAlbumView);
            Add<PartOfPlaylist>(GetPlaylistView);
            Add<PartOfRel>(GetCharacterRel);
            Add<PartOfSurvey>(GetSurvey);
            Add<PartOfSentence>(GetSentenceView);
            Add<PartOfPrototype>(GetPrototypeView);
            Add<PartOfApprise>(GetAppriseView);
            Add<PartOfStickyNote>(GetStickyNote);
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