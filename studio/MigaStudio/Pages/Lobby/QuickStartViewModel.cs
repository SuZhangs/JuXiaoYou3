using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Lobby
{
    public class QuickStartViewModel : PageViewModel
    {
        private readonly IDatabaseManager _databaseManager;
        
        private string _name;
        private string _foreignName;
        private string _author;
        private string _cover;
        private string _icon;
        private string _intro;

        public QuickStartViewModel(IDatabaseManager manager)
        {
            _databaseManager = manager;
            
            CreateCommand      = new AsyncRelayCommand(CreateImpl);
            OpenCommand        = new AsyncRelayCommand(OpenImpl);
            SelectIconCommand  = new AsyncRelayCommand(SelectIconImpl);
            SelectCoverCommand = new AsyncRelayCommand(SelectCoverImpl);
            UpgradeCommand     = new AsyncRelayCommand(UpgradeImpl);
        }

        private async Task CreateImpl()
        {
            
        }
        
        private async Task OpenImpl()
        {
            
        }
        
        private async Task UpgradeImpl()
        {
            
        }
        
        private async Task SelectIconImpl()
        {
            
        }
        
        private async Task SelectCoverImpl()
        {
            
        }

        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author
        {
            get => _author;
            set => SetValue(ref _author, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ForeignName"/> 属性。
        /// </summary>
        public string ForeignName
        {
            get => _foreignName;
            set => SetValue(ref _foreignName, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Icon"/> 属性。
        /// </summary>
        public string Icon
        {
            get => _icon;
            set => SetValue(ref _icon, value);
        }
        
        public AsyncRelayCommand SelectCoverCommand { get; }
        public AsyncRelayCommand SelectIconCommand { get; }
        public AsyncRelayCommand CreateCommand { get; }
        public AsyncRelayCommand OpenCommand { get; }
        public AsyncRelayCommand UpgradeCommand { get; }
    }
}