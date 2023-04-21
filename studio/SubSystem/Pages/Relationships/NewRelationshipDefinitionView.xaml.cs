using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    [Connected(View = typeof(NewRelationshipDefinitionView), ViewModel = typeof(NewRelationshipDefinitionViewModel))]
    public partial class NewRelationshipDefinitionView
    {
        public NewRelationshipDefinitionView()
        {
            InitializeComponent();
        }
    }
}