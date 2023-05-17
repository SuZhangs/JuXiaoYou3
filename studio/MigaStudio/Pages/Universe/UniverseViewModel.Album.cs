using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    partial class UniverseViewModel
    {
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
                            var r         = await ImageUtilities.Thumbnail(ImageEngine, fileName);
                            var thumbnail = r.Value;
                            _threadSafeAdding.OnNext(thumbnail);
                        }
                        catch (Exception ex)
                        {
                            await this.Error(ex.Message.SubString());
                        }
                    }
                    
                    Save();
                    this.Successful(SubSystemString.OperationOfAddIsSuccessful);
                });
            }
        }

        private async Task RemoveAlbumImpl(Album part)
        {
            if (part is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            PictureCollection.Remove(part);
            _databaseProperty.Album.Remove(part);
            Save();
        }

        private void OpenAlbumImpl(Album part)
        {
            if (part is null)
            {
                return;
            }

            var fileName = ImageEngine.GetFileName(part.Source);
            SubSystem.ImageView(fileName);
        }

        private void ShiftDownAlbumImpl(Album album)
        {
            PictureCollection.ShiftDown(album);
            Save();
        }

        private void ShiftUpAlbumImpl(Album album)
        {
            PictureCollection.ShiftUp(album);
            Save();
        }

        #region Album

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
        public RelayCommand<Album> OpenAlbumCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Album> RemoveAlbumCommand { get; }

        #endregion
    }
}