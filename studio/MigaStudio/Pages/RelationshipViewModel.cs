using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RelationshipViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreateGalleryFeature<RelationshipDefinitionViewModel>("global.RelationshipDefinition");
            CreateGalleryFeature<CharacterRelationshipViewModel>("global.CharacterRelationship");
        }
    }
}