using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(HomePage), ViewModel = typeof(HomeViewModel))]
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}