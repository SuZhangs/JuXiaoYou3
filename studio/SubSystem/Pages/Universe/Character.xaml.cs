using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(CharacterPage), ViewModel = typeof(CharacterViewModel))]
    public partial class CharacterPage
    {
        public CharacterPage()
        {
            InitializeComponent();
        }
    }
}