using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(SpacePage), ViewModel = typeof(SpaceViewModel))]
    public partial class SpacePage
    {
        public SpacePage()
        {
            InitializeComponent();
        }
    }
}