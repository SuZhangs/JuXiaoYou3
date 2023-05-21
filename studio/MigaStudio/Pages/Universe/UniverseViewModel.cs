using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using Acorisoft.FutureGL.MigaStudio.Resources.Converters;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using NLog;

// ReSharper disable All

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public partial class UniverseViewModel : TabViewModel
    {
        private readonly Subject<Album>   _threadSafeAdding;
        private readonly DatabaseProperty _databaseProperty;

        private Album _selectedAlbum;
        private bool  _exitOperation;

        public UniverseViewModel()
        {
            PictureCollection = new ObservableCollection<Album>();
            DatabaseManager   = Studio.DatabaseManager();
            ImageEngine       = DatabaseManager.GetEngine<ImageEngine>();
            _databaseProperty = Database.Get<DatabaseProperty>();
            _threadSafeAdding = new Subject<Album>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 if (PictureCollection.Any(y => y.Source == x.Source))
                                 {
                                     this.WarningNotification(Language.ItemDuplicatedText);
                                     return;
                                 }

                                 PictureCollection.Add(x);
                                 _databaseProperty.Album.Add(x);
                                 SelectedAlbum ??= PictureCollection.FirstOrDefault();
                                 Save();
                             })
                             .DisposeWith(Collector);

            CloseDatabaseCommand  = AsyncCommand(CloseDatabaseImpl);
            ChangeAvatarCommand   = AsyncCommand(ChangeAvatarImpl);
            EditCommand           = Command(() => Controller.New<UniverseEditorViewModel>());
            AddAlbumCommand       = AsyncCommand(AddAlbumImpl);
            RemoveAlbumCommand    = AsyncCommand<Album>(RemoveAlbumImpl, HasItem);
            ShiftUpAlbumCommand   = Command<Album>(ShiftUpAlbumImpl, HasItem);
            ShiftDownAlbumCommand = Command<Album>(ShiftDownAlbumImpl, HasItem);
            OpenAlbumCommand      = Command<Album>(OpenAlbumImpl, HasItem);
        }

        protected override void OnStart()
        {
            if (_databaseProperty.Album is null)
            {
                _databaseProperty.Album = new ObservableCollection<Album>();
                Save();
            }

            PictureCollection.AddMany(_databaseProperty.Album, true);
        }

        public override void Stop()
        {
            if (_exitOperation)
            {
                return;
            }
            Save();
        }

        private async Task CloseDatabaseImpl()
        {
            _exitOperation = true;
            var ss         = Xaml.Get<SystemSetting>();
            var context    = Controller.Context;
            var controller = (TabController)context.MainController;
            controller.Reset();
            controller = (TabController)Controller.Context
                                                  .ControllerMaps[AppViewModel.IdOfQuickStartController];
            ConverterPool.Avatar
                         .Reset();
            ss.RepositorySetting
              .LastRepository = null;
            await ss.SaveAsync();
            await DatabaseManager.CloseAsync();
            context.SwitchController(controller);
        }

        private void Save()
        {
            Database.Set(_databaseProperty);
        }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _databaseProperty.Intro;
            set
            {
                _databaseProperty.Intro = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="ForeignName"/> 属性。
        /// </summary>
        public string ForeignName
        {
            get => _databaseProperty.ForeignName;
            set
            {
                _databaseProperty.ForeignName = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        public string Author
        {
            get => _databaseProperty.Author;
            set
            {
                _databaseProperty.Author = value;
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Name
        {
            get => _databaseProperty.Name;
            set
            {
                _databaseProperty.Name = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Icon
        {
            get => _databaseProperty.Icon;
            set
            {
                _databaseProperty.Icon = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (ImageEngine.HasFile(md5))
            {
                var fr = ImageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = ImageUtilities.GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                ImageEngine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ImageEngine.AddFile(record);
            }

            Icon = avatar;
        }

        //---------------------------------------------
        //
        // Keywords
        //
        //---------------------------------------------


        public DocumentType Type => DocumentType.None;
        
        public AsyncRelayCommand CloseDatabaseCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ChangeAvatarCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Album> PictureCollection { get; init; }

        public IDatabase Database => DatabaseUtilities.Database;
        public IDatabaseManager DatabaseManager { get; }
        public ImageEngine ImageEngine { get; }

        public override bool Uniqueness => true;
    }
}