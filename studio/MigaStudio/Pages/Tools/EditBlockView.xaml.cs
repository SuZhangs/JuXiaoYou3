using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{
    [Connected(View = typeof(EditBlockView), ViewModel = typeof(EditBlockViewModel))]
    public partial class EditBlockView
    {
        public EditBlockView()
        {
            InitializeComponent();
        }
    }
}