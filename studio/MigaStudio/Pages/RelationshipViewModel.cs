using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RelationshipViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreatePageFeature<RelationshipDefinitionViewModel>( "global.RelationshipDefinition");
            CreatePageFeature<CharacterRelationshipViewModel>("global.CharacterRelationship");
        }
    }
}