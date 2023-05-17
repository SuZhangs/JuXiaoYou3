using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Composes;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public delegate FrameworkElement ViewFactory<TViewModel>(TViewModel owner, object parameter);
    
    public static partial class ServiceViewContainer
    {
        private static readonly IViewFactoryService _service = new ViewFactoryService();
        
        
        
        public static void Initialize()
        {
            _service.Manual<PartOfRel>(GetCharacterRel);
            UseDataPart<PartOfAlbum, AlbumPartViewModel, AlbumPartView>();
            UseDataPart<PartOfPlaylist, PlaylistPartViewModel, PlaylistPartView>();
            UseDataPart<PartOfSurvey, SurveyPartViewModel, SurveyPartView>();
            UseDataPart<PartOfSentence, SentencePartViewModel, SentencePartView>();
            UseDataPart<PartOfPrototype, PrototypePartViewModel, PrototypePartView>();
            UseDataPart<PartOfAppraise, AppraisePartViewModel, AppraisePartView>();
            UseDataPart<PartOfStickyNote, StickyNotePartViewModel, StickyNotePartView>();
            UseDetailSetting();
            
            
            Browse<UniversalIntroduction>(GetUniversalView);
            Browse<SpaceConceptOverview>(GetSpaceConceptOverviewView);
            Browse<PropertyOverview>(GetPropertyOverviewView);
            Browse<OtherIntroduction>(GetOtherView);
            Browse<SpaceConcept>(GetSpaceConceptView);
            Browse<BrowsableProperty>(GetBrowsablePropertyView);
            Browse<DeclarationConcept>(GetDeclarationConceptView);
            Browse<RarityConcept>(GetRarityConceptView);
        }
    }
}