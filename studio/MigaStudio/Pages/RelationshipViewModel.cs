using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RelationshipViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreatePageFeature<RelativePresetViewModel>( "global.RelationshipDefinition");
            CreatePageFeature<CharacterRelationshipViewModel>("global.CharacterRelationship");
        }
    }
}