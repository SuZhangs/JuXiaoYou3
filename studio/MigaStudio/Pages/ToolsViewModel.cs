using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ToolsViewModelProxy : BindingProxy<ToolsViewModel>
    {
    }

    public class ToolsViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreatePageFeature<CharacterRelationshipViewModel>(string.Empty, "__CharacterRelationship", null);
            CreateDialogFeature<DirectoryManagerViewModel>(string.Empty, "__DirectoryStatistic", null);
            CreateDialogFeature<RepairToolViewModel>(string.Empty, "global.repair", null);
            CreatePageFeature<KeywordViewModel>(string.Empty, "__Keywords", null);
            CreatePageFeature<BookmarkViewModel>(string.Empty, "__Bookmark", null);
        }
    }
}