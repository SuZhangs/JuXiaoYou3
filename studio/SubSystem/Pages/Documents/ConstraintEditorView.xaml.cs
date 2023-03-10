using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{

    [Connected(View = typeof(ConstraintEditorView), ViewModel = typeof(CharacterConstraintViewModel))]
    [Connected(View = typeof(ConstraintEditorView), ViewModel = typeof(ItemConstraintViewModel))]
    public partial class ConstraintEditorView : UserControl
    {
        public ConstraintEditorView()
        {
            InitializeComponent();
        }
    }
}