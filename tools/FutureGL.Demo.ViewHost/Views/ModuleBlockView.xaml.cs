using System.Windows.Controls;

namespace ViewHost.Views
{
    [Connected(View = typeof(ModuleBlockView), ViewModel = typeof(ModuleBlockViewModel))]
    public partial class ModuleBlockView : UserControl
    {
        public ModuleBlockView()
        {
            InitializeComponent();
        }
    }
}