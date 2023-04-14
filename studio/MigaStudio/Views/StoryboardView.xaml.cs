using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{

    [Connected(View = typeof(StoryboardViewPage), ViewModel = typeof(StoryboardController))]
    public partial class StoryboardViewPage
    {
        public StoryboardViewPage()
        {
            InitializeComponent();
        }
    }
}