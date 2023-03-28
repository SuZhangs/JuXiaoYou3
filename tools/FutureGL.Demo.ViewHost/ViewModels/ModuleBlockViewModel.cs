using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;

namespace Acorisoft.FutureGL.Demo.ViewHost.ViewModels
{
    [Name("模组")]
    public class ModuleBlockViewModel : PageViewModel
    {
        public ModuleBlockViewModel()
        {
            Blocks = new ObservableCollection<ModuleBlockEditUI>(
                ModuleBlockFactory.CreateBlocks()
                                  .Select(ModuleBlockFactory.GetEditUI));
        }
        public ObservableCollection<ModuleBlockEditUI> Blocks { get; init; }
    }
}