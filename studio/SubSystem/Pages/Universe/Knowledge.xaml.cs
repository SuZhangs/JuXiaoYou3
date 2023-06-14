using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(KnowledgePage), ViewModel = typeof(KnowledgeViewModel))]
    public partial class KnowledgePage
    {
        public KnowledgePage()
        {
            InitializeComponent();
        }
    }
}