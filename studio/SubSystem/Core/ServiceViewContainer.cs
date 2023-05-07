using Acorisoft.FutureGL.MigaStudio.Pages.Composes;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
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
        }
    }
}