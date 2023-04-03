﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Threading;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class Album : StorageUIObject
    {
        public string Thumbnail { get; init; }
        public string Source { get; init; }
    }

    public class AlbumPartViewModel : ViewModelBase
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
                             })
                             .DisposeWith(Collector);
            AddAlbumCommand       = AsyncCommand(AddAlbumImpl);
            RemoveAlbumCommand    = AsyncCommand<Album>(RemoveAlbumImpl, HasItem);
            ShiftUpAlbumCommand   = Command<Album>(ShiftUpAlbumImpl, HasItem);
            ShiftDownAlbumCommand = Command<Album>(ShiftDownAlbumImpl, HasItem);
            OpenAlbumCommand      = AsyncCommand<Album>(OpenAlbumImpl, HasItem);
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
                session.Update(SubSystemString.ImageProcessing);

                await Task.Run(async () =>
                {
                    await Task.Delay(300);

                    foreach (var fileName in opendlg.FileNames)
                    {
                        try
                        {
                            var r = await ImageUtilities.Thumbnail(ImageEngine, fileName);
                            var (source, thumbnail) = r.Value;
                            var album = new Album
                            {
                                Id        = ID.Get(),
                                Source    = source,
                                Thumbnail = thumbnail
                            };
                            _threadSafeAdding.OnNext(album);
                        }
                        catch (Exception ex)
                        {
                            Error(ex.Message.SubString());
                        }
                    }
                });
            }


            //
            //
            SelectedAlbum ??= Collection.FirstOrDefault();
            Successful(SubSystemString.OperationOfAddIsSuccessful);
            Save();
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

            if (Detail.DataBags.ContainsKey(Data))
            {
                Detail.DataBags[Data] = payload;
            }
            else
            {
                Detail.DataBags.Add(Data, payload);
            }

            EditorViewModel.SetDirtyState();
        }

        private const string Data = "_d";

        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorVMBase EditorViewModel { get; init; }

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
                RemoveAlbumCommand.NotifyCanExecuteChanged();
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