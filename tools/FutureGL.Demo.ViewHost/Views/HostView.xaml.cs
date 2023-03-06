using System.Windows.Controls;
using Acorisoft.FutureGL.Demo.ViewModels;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(HostView), ViewModel = typeof(HostViewModel))]
    public partial class HostView : UserControl
    {
        public HostView()
        {
            InitializeComponent();
            Xaml.Get<IDialogService>().Dialog<Document, NewDocumentWizardViewModel>();
        }
    }
}