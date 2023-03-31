using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public interface IPreviewBlockViewModel
    {
        public string PreviewFor { get; }
        public string PreviewIntro { get; }
        public string PreviewContractList { get; }
        public string PreviewAuthorList { get; }
        public string PreviewName { get; }
        public ObservableCollection<ModuleBlockDataUI> PreviewBlocks { get; }
    }
}