using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public partial class UniverseEditorViewModel : TabViewModel
    {
        private IBrowsable       _selectedBrowsableElement;
        private FrameworkElement _selectedView;

        public UniverseEditorViewModel()
        {
            BrowsableElements = new ObservableCollection<IBrowsableRoot>();
            InitializeBrowsableElements();
        }
    }
}