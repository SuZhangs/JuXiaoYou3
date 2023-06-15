using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItem   _selectedItem;
        private ITabViewModel _selectedViewModel;
        
        public FantasyProjectStartupViewModel()
        {
            ProjectElements  = new ObservableCollection<ProjectItem>();
            DocumentElements = new ObservableCollection<ProjectItem>();
            OtherElements    = new ObservableCollection<ProjectItem>();

            ProjectEngine = Studio.Engine<ProjectEngine>();
            
            CreateProjectItems();
        }

        protected override void OnStart()
        {
            //
            // 默认
            SelectedItem = ProjectElements.First();
            
            //
            // 
            Aggregate();
            
            //
            //
            base.OnStart();
        }

        private void Aggregate()
        {
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedItem"/> 属性。
        /// </summary>
        public ProjectItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetValue(ref _selectedItem, value);
                SelectedViewModel = _selectedItem?.ViewModel;
                
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedViewModel"/> 属性。
        /// </summary>
        public ITabViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                _selectedViewModel?.Suspend();
                SetValue(ref _selectedViewModel, value);
                _selectedViewModel?.Start();
            }
        }

        public ObservableCollection<ProjectItem> ProjectElements { get; }
        public ObservableCollection<ProjectItem> DocumentElements { get; }
        public ObservableCollection<ProjectItem> OtherElements { get; }
        public ProjectEngine ProjectEngine { get; }
    }
}