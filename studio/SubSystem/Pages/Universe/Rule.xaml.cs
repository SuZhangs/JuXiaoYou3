using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(RulePage), ViewModel = typeof(RuleViewModel))]
    public partial class RulePage
    {
        public RulePage()
        {
            InitializeComponent();
        }
    }
}