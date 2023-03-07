using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SubSystemDialogs
    {
        public static Task<Op<DocumentCache>> NewDocumentWizard()
        {
            return Xaml.Get<IDialogService>().Dialog<DocumentCache, NewDocumentWizardViewModel>();
        }
    }
}