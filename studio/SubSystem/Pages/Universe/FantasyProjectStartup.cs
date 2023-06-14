using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItemBase _selectedItem;

        public FantasyProjectStartupViewModel()
        {
            ProjectItems = new ObservableCollection<ProjectItemBase>();
            CreateProjectItems(ProjectItems);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedItem"/> 属性。
        /// </summary>
        public ProjectItemBase SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }
        
        public ObservableCollection<ProjectItemBase> ProjectItems { get; }
    }
}