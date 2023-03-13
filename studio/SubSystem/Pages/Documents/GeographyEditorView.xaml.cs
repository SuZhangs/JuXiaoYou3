using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{

    [Connected(View = typeof(GeographyEditorView), ViewModel = typeof(GeographyDocumentViewModel))]
    public partial class GeographyEditorView
    {
        public GeographyEditorView()
        {
            InitializeComponent();
        }
    }
}