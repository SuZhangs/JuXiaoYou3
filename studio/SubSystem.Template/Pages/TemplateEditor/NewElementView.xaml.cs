using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    [Connected(View = typeof(NewElementView), ViewModel = typeof(NewElementViewModel))]
    public partial class NewElementView
    {
        public NewElementView()
        {
            InitializeComponent();
        }
    }
}