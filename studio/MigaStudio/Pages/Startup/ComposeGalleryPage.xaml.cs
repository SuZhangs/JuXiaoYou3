using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ComposeGalleryPage), ViewModel = typeof(ComposeGalleryViewModel))]
    public partial class ComposeGalleryPage
    {
        public ComposeGalleryPage()
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