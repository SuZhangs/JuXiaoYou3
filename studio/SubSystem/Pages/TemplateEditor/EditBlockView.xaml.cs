using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
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