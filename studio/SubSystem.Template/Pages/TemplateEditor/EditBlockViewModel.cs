using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    public class EditBlockViewModel : DialogViewModel
    {
        public static Task<Op<ModuleBlockEditUI>> New(ModuleBlockEditUI element)
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<ModuleBlockEditUI>(new EditBlockViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               element
                           }
                       });
        }
    }
}