using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(ItemPage), ViewModel = typeof(ItemViewModel))]
    public partial class ItemPage
    {
        public ItemPage()
        {
            InitializeComponent();
        }
    }
}