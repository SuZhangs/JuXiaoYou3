using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    [Connected(View = typeof(DetailPartSelectorView), ViewModel = typeof(DetailPartSelectorViewModel))]
    public partial class DetailPartSelectorView
    {
        public DetailPartSelectorView()
        {
            InitializeComponent();
        }
    }
}