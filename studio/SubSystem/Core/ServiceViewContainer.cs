using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Composes;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public delegate UserControl ViewFactory<TViewModel>(TViewModel owner, object parameter);
    
    public static partial class ServiceViewContainer
    {
        
        public static void Initialize()
        {
            Add<PartOfAlbum>(GetAlbumView);
            Add<PartOfPlaylist>(GetPlaylistView);
            Add<PartOfRel>(GetCharacterRel);
            Add<PartOfSurvey>(GetSurvey);
            Add<PartOfSentence>(GetSentenceView);
            Add<PartOfPrototype>(GetPrototypeView);
            Add<PartOfAppraise>(GetAppraiseView);
            Add<PartOfStickyNote>(GetStickyNote);
            Add<DetailPartSettingPlaceHolder>(GetDetailSetting);
            
            
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