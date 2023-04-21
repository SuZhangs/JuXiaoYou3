using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{

    [Connected(View = typeof(RelationshipDefinitionPage), ViewModel = typeof(RelationshipDefinitionViewModel))]
    public partial class RelationshipDefinitionPage
    {
        public RelationshipDefinitionPage()
        {
            InitializeComponent();
        }
    }
}