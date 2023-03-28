using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    [Connected(View = typeof(ModuleSelectorView), ViewModel = typeof(ModuleSelectorViewModel))]
    public partial class ModuleSelectorView
    {
        public ModuleSelectorView()
        {
            InitializeComponent();
        }
    }
}