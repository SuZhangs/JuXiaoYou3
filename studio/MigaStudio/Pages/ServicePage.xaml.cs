using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ServicePage), ViewModel = typeof(ServiceViewModel))]
    public partial class ServicePage
    {
        public ServicePage()
        {
            InitializeComponent();
        }
    }
}