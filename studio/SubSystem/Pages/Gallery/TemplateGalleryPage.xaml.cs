using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{

    [Connected(View = typeof(TemplateGalleryPage), ViewModel = typeof(TemplateGalleryViewModel))]
    public partial class TemplateGalleryPage:ForestUserControl 
    {
        public TemplateGalleryPage()
        {
            InitializeComponent();
        }
    }
}