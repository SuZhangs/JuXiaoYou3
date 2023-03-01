using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(SettingPage), ViewModel = typeof(SettingViewModel))]
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            InitializeComponent();
        }
    }
}