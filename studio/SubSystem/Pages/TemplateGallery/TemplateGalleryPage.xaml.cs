using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateGallery
{

    [Connected(View = typeof(TemplateGalleryPage), ViewModel = typeof(TemplateGalleryViewModel))]
    public partial class TemplateGalleryPage
    {
        public TemplateGalleryPage()
        {
            InitializeComponent();
        }
    }
}