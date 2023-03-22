using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{
    public class EditBlockViewModel : DialogViewModel
    {
        public static Task<Op<ModuleBlock>> New(ModuleBlock element)
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<ModuleBlock>(new EditBlockViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               element
                           }
                       });
        }
    }
}