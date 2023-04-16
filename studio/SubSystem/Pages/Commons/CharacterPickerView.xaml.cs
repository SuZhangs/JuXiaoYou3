using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(CharacterPickerView), ViewModel = typeof(DocumentPickerViewModel))]
    public partial class CharacterPickerView
    {
        public CharacterPickerView()
        {
            InitializeComponent();
        }
    }
}