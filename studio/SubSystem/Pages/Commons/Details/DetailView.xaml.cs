using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public partial class DetailPartView : UserControl
    {
        public DetailPartView()
        {
            InitializeComponent();
        }
    }

    public abstract class DetailViewModel<TDetail> : ViewModelBase where TDetail : PartOfDetail
    {
        public TDetail Detail { get; init; }
        public DocumentEditorBase Owner { get; init; }
    }
}