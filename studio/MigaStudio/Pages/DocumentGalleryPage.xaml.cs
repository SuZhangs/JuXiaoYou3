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

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (TextBox)sender;

            if (string.IsNullOrEmpty(text.Text))
            {
                var vm = ViewModel<DocumentGalleryViewModel>();
                vm.IsFiltering = false;
                vm.SearchPage();
            }
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