using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class AlbumPartViewModel : DetailViewModel<PartOfAlbum>
    {
        private readonly Subject<Album> _threadSafeAdding;
        private          Album          _selectedAlbum;

        
        public AlbumPartViewModel()
        {
            Collection            = new ObservableCollection<Album>();
            ImageEngine = Xaml.Get<IDatabaseManager>()
                              .GetEngine<ImageEngine>();
            _threadSafeAdding     = new Subject<Album>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 if (Collection.Any(y => y.Source == x.Source))
                                 {
                                     Warning(Language.ItemDuplicatedText);
                                     return;
                                 }
                                 Collection.Add(x);
                                 Detail!.Items.Add(x);
                                 SelectedAlbum ??= Collection.FirstOrDefault();
                             })
                             .DisposeWith(Collector);
            AddAlbumCommand       = AsyncCommand(AddAlbumImpl);
            RemoveAlbumCommand    = AsyncCommand<Album>(RemoveAlbumImpl, HasItem);
            ShiftUpAlbumCommand   = Command<Album>(ShiftUpAlbumImpl, HasItem);
            ShiftDownAlbumCommand = Command<Album>(ShiftDownAlbumImpl, HasItem);
            OpenAlbumCommand      = AsyncCommand<Album>(OpenAlbumImpl, HasItem);
        }

        public override void Start()
        {
            base.Start();
            
            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOfAlbum为空");
                return;
            }
            Collection.AddRange(Detail.Items);
        }

        private async Task AddAlbumImpl()
        {
            var opendlg = FileIO.Open(SubSystemString.ImageFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                session.Update(SubSystemString.Processing);

                await Task.Run(async () =>
                {
                    await Task.Delay(300);

                    foreach (var fileName in opendlg.FileNames)
                    {
                        try
                        {
                            var r = await ImageUtilities.Thumbnail(ImageEngine, fileName);
                            var thumbnail = r.Value;
                            _threadSafeAdding.OnNext(thumbnail);
                        }
                        catch (Exception ex)
                        {
                            Error(ex.Message.SubString());
                        }
                    }
                    Save();
                    Successful(SubSystemString.OperationOfAddIsSuccessful);
                });
            }
        }

        private async Task RemoveAlbumImpl(Album part)
        {
            if (part is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Collection.Remove(part);
            Detail.Items.Remove(part);
            Save();
        }

        private async Task OpenAlbumImpl(Album part)
        {
            if (part is null)
            {
                return;
            }

            var fileName = ImageEngine.GetFileName(part.Source);
            await SubSystem.ImageView(fileName);
        }

        private void ShiftDownAlbumImpl(Album album)
        {
            Collection.ShiftDown(album);
            Save();
        }

        private void ShiftUpAlbumImpl(Album album)
        {
            Collection.ShiftUp(album);
            Save();
        }

        private void Save()
        {
            Owner.SetDirtyState();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Album> Collection { get; init; }

        public ImageEngine ImageEngine { get; }


        /// <summary>
        /// 获取或设置 <see cref="SelectedAlbum"/> 属性。
        /// </summary>
        public Album SelectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                SetValue(ref _selectedAlbum, value);
                RemoveAlbumCommand.NotifyCanExecuteChanged();
                ShiftDownAlbumCommand.NotifyCanExecuteChanged();
                ShiftUpAlbumCommand.NotifyCanExecuteChanged();
                OpenAlbumCommand.NotifyCanExecuteChanged();
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddAlbumCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Album> ShiftUpAlbumCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Album> ShiftDownAlbumCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Album> OpenAlbumCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Album> RemoveAlbumCommand { get; }
    }
}