using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{

    [Connected(View = typeof(TemplateEditorPage), ViewModel = typeof(TemplateEditorViewModel))]
    public partial class TemplateEditorPage
    {
        public TemplateEditorPage()
        {
            InitializeComponent();
        }
    }
}