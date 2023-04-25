using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(PlanetEditorPage), ViewModel = typeof(PlanetEditorViewModel))]
    public partial class PlanetEditorPage
    {
        public PlanetEditorPage()
        {
            InitializeComponent();
        }
    }
}