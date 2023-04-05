using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{

    [Connected(View = typeof(CharacterRelationshipPage), ViewModel = typeof(CharacterRelationshipViewModel))]
    public partial class CharacterRelationshipPage
    {
        public CharacterRelationshipPage()
        {
            InitializeComponent();
        }
    }
}