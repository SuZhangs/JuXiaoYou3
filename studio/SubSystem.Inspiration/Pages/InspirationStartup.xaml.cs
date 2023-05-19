using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Inspiration.Pages
{

    [Connected(View = typeof(InspirationStartupPage), ViewModel = typeof(InspirationStartupViewModel))]
    public partial class InspirationStartupPage
    {
        public InspirationStartupPage()
        {
            InitializeComponent();
        }
    }
}