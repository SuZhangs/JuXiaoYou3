using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{

    [Connected(View = typeof(GeographyEditorPage), ViewModel = typeof(GeographyDocumentViewModel))]
    public partial class GeographyEditorPage
    {
        public GeographyEditorPage()
        {
            InitializeComponent();
        }
    }
}