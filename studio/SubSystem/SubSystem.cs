using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using ImageEditViewModel = Acorisoft.FutureGL.MigaStudio.Pages.Commons.ImageEditViewModel;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public static class SubSystem
    {
        public static Task<Op<DocumentCache>> NewDocumentWizard()
        {
            return Xaml.Get<IDialogService>().Dialog<DocumentCache, NewDocumentWizardViewModel>();
        }

        public static void InstallViews()
        {
            Xaml.InstallView<ImageEditView, ImageEditViewModel>();
            Xaml.InstallView<NewDocumentWizard, NewDocumentWizardViewModel>();
        }
    }
}