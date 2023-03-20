using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(SettingPage), ViewModel = typeof(SettingViewModel))]
    public partial class SettingPage:ForestUserControl 
    {
        public SettingPage()
        {
            InitializeComponent();
        }
    }
}