using System.Windows.Controls;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(DocumentGalleryPage), ViewModel = typeof(DocumentGalleryViewModel))]
    public partial class DocumentGalleryPage
    {
        public DocumentGalleryPage()
        {
            InitializeComponent();
        }


        private void SearchPage_OnKeyUp(object sender, KeyEventArgs e)
        {
            var vm = ViewModel<DocumentGalleryViewModel>();
            
            if (e.Key == Key.Enter)
            {
                vm.SearchPage();
            }
            else if (e.Key == Key.Escape)
            {
                vm.FilterString = string.Empty;
                vm.IsFiltering  = false;
                vm.SearchPage();
            }
        }
    }
}