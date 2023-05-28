using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations.Pages.Communications
{

    [Connected(View = typeof(CharacterChannelPage), ViewModel = typeof(CharacterChannelViewModel))]
    public partial class CharacterChannelPage
    {
        public CharacterChannelPage()
        {
            InitializeComponent();
        }
    }
}