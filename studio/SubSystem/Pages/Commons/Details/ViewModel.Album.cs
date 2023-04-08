using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class AlbumPartViewModel : KeyValueViewModel
    {
        private readonly Subject<Album> _threadSafeAdding;
        
        public AlbumPartViewModel()
        {
            Collection            = new ObservableCollection<Album>();
            ImageEngine = Xaml.Get<IDatabaseManager>()
                              .GetEngine<ImageEngine>();
            _threadSafeAdding     = new Subject<Album>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 Collection.Add(x);
                                 SelectedAlbum ??= Collection.FirstOrDefault();
                                 Successful(SubSystemString.OperationOfAddIsSuccessful);
                                 Save();
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
            if (Detail.DataBags.ContainsKey(Data))
            {
                var payload = Detail.DataBags[Data];
                var list    = JSON.FromJson<ObservableCollection<Album>>(payload);
                Collection.AddRange(list);
            }
            
            base.Start();
        }

        private async Task AddAlbumImpl()
        {
            var opendlg = new VistaOpenFileDialog
            {
                Filter      = SubSystemString.ImageFilter,
                Multiselect = true
            };


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
            var payload = JSON.Serialize(Collection);
            base[Data, Detail.DataBags] = payload;
            EditorViewModel.SetDirtyState();
        }

        private const string Data = "_d";

        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorBase EditorViewModel { get; init; }

        /// <summary>
        /// 文档
        /// </summary>
        public Document Document { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCache DocumentCache { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public PartOfAlbum Detail { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Album> Collection { get; init; }

        public ImageEngine ImageEngine { get; }
        private Album _selectedAlbum;

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