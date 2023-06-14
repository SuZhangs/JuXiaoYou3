using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(GeographyPage), ViewModel = typeof(GeographyViewModel))]
    public partial class GeographyPage
    {
        public GeographyPage()
        {
            InitializeComponent();
        }
    }
}