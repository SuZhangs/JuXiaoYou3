using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(SkillPage), ViewModel = typeof(SkillViewModel))]
    public partial class SkillPage
    {
        public SkillPage()
        {
            InitializeComponent();
        }
    }
}