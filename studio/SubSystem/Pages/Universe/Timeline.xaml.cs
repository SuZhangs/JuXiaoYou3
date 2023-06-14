using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(TimelinePage), ViewModel = typeof(TimelineViewModel))]
    public partial class TimelinePage
    {
        public TimelinePage()
        {
            InitializeComponent();
        }
    }
}