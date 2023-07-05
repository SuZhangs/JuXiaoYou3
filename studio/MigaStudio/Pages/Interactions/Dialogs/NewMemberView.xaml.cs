using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    [Connected(View = typeof(NewMemberView), ViewModel = typeof(NewMemberViewModel))]
    public partial class NewMemberView
    {
        public NewMemberView()
        {
            InitializeComponent();
        }
    }
}