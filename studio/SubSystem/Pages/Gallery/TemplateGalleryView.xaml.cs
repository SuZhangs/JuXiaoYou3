using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{

    [Connected(View = typeof(TemplateGalleryView), ViewModel = typeof(TemplateGalleryViewModel))]
    public partial class TemplateGalleryView:ForestUserControl 
    {
        public TemplateGalleryView()
        {
            InitializeComponent();
        }
    }
}