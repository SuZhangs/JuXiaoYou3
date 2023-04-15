using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(BookmarkPage), ViewModel = typeof(BookmarkViewModel))]
    public partial class BookmarkPage
    {
        public BookmarkPage()
        {
            InitializeComponent();
        }
    }
}