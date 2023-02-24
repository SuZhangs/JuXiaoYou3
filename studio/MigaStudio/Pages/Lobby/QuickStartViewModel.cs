using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Models;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Lobby
{
    public class QuickStartViewModel : PageViewModel
    {
        private string _name;
        private string _foreignName;
        private string _author;
        private string _cover;
        private string _icon;
        private string _intro;

        public QuickStartViewModel()
        {
            CreateCommand      = AsyncCommand(nameof(CreateCommand), CreateImpl, CanCreate);
            OpenCommand        = AsyncCommand(nameof(OpenCommand), OpenImpl);
            SelectIconCommand  = AsyncCommand(nameof(SelectIconCommand), SelectIconImpl);
            SelectCoverCommand = AsyncCommand(nameof(SelectCoverCommand), SelectCoverImpl);
            UpgradeCommand     = AsyncCommand(nameof(UpgradeCommand), UpgradeImpl);
        }

        private bool CanCreate() => !string.IsNullOrEmpty(Author) &&
                                    !string.IsNullOrEmpty(Name) &&
                                    !string.IsNullOrEmpty(ForeignName);

        private async Task CreateImpl()
        {
            var opendlg = new VistaFolderBrowserDialog();

            if (opendlg.ShowDialog(Application.Current.MainWindow) != true)
            {
                return;
            }

            var folder = opendlg.SelectedPath;
            var dbMgr = Xaml.Get<IDatabaseManager>();
            var result = await dbMgr.CreateAsync(folder, new DatabaseProperty
            {
                Name        = Name,
                ForeignName = ForeignName,
                Author      = Author,
                Icon        = Icon,
                Cover       = Cover,
            });
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