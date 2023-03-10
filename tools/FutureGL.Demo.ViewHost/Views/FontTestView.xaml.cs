using System.Windows.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{

    [Connected(View = typeof(FontTestView), ViewModel = typeof(FontTestViewModel))]
    public partial class FontTestView : UserControl
    {
        public FontTestView()
        {
            InitializeComponent();
        }
    }
}