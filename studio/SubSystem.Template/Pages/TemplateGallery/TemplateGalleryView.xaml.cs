using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateGallery
{

    [Connected(View = typeof(TemplateGalleryView), ViewModel = typeof(TemplateGalleryViewModel))]
    public partial class TemplateGalleryView
    {
        public TemplateGalleryView()
        {
            InitializeComponent();
        }
    }
}