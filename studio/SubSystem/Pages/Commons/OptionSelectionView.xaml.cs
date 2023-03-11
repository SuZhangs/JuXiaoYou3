using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(OptionSelectionView), ViewModel = typeof(OptionSelectionViewModel))]
    public partial class OptionSelectionView : UserControl
    {
        public OptionSelectionView()
        {
            InitializeComponent();
        }
    }
}