using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItem _selectedItem;

        public FantasyProjectStartupViewModel()
        {
            ProjectElements  = new ObservableCollection<ProjectItem>();
            DocumentElements = new ObservableCollection<ProjectItem>();
            OtherElements    = new ObservableCollection<ProjectItem>();
            CreateProjectItems();
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedItem"/> 属性。
        /// </summary>
        public ProjectItem SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }
        
        public ObservableCollection<ProjectItem> ProjectElements { get; }
        public ObservableCollection<ProjectItem> DocumentElements { get; }
        public ObservableCollection<ProjectItem> OtherElements { get; }
    }
}