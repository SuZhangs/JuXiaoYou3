using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;

namespace Acorisoft.FutureGL.Demo.ViewHost.ViewModels
{
    [Name("模组")]
    public class ModuleBlockViewModel : PageViewModel
    {
        public ModuleBlockViewModel()
        {
            Blocks = new ObservableCollection<ModuleBlockDataUI>(
                ModuleBlockFactory.CreateBlocks()
                                  .Where(x => x is not ColorBlock)
                                  .Select(ModuleBlockFactory.GetDataUI));
        }
        public ObservableCollection<ModuleBlockDataUI> Blocks { get; init; }
    }
}