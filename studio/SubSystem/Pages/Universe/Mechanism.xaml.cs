using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(MechanismPage), ViewModel = typeof(MechanismViewModel))]
    public partial class MechanismPage
    {
        public MechanismPage()
        {
            InitializeComponent();
        }
    }
}