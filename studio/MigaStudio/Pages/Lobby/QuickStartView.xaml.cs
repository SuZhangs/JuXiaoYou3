using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Lobby
{
    [Connected(View = typeof(QuickStartView), ViewModel = typeof(QuickStartViewModel))]
    public partial class QuickStartView : UserControl
    {
        public QuickStartView()
        {
            InitializeComponent();
        }
    }
}