using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(MaterialEditorPage), ViewModel = typeof(MaterialEditorViewModel))]
    public partial class MaterialEditorPage
    {
        public MaterialEditorPage()
        {
            InitializeComponent();
        }
    }
}