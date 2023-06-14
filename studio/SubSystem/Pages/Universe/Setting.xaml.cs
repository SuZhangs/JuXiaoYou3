using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(SettingPage), ViewModel = typeof(SettingViewModel))]
    public partial class SettingPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }
    }
}