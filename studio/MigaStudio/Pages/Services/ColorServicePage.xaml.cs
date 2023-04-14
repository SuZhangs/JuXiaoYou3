using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ColorServicePage), ViewModel = typeof(ColorServiceViewModel))]
    public partial class ColorServicePage
    {
        public ColorServicePage()
        {
            InitializeComponent();
        }
    }
}