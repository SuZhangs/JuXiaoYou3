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
    public class AlbumPartViewModel : DetailViewModel<PartOfAlbum, Album>
    {
        public AlbumPartViewModel()
        {
            Collection = new ObservableCollection<Album>();
            ImageEngine = Xaml.Get<IDatabaseManager>()
                              .GetEngine<ImageEngine>();
            OpenCommand = Command<Album>(OpenAlbumImpl, HasItem);
        }

        protected override async Task AddItem()
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
                            Sync(thumbnail);
                        }
                        catch (Exception ex)
                        {
                            Error(ex.Message.SubString());
                        }
                    }

                    SaveOperation();
                    Successful(SubSystemString.OperationOfAddIsSuccessful);
                });
            }
        }

        protected override async Task RemoveItem(Album part)
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
            SaveOperation();
        }

        protected override void ShiftDownItem(Album album)
        {
            Collection.ShiftDown(album);
            SaveOperation();
        }

        protected override void ShiftUpItem(Album album)
        {
            Collection.ShiftUp(album);
            SaveOperation();
        }

        protected override void OnCollectionChanged(Album x)
        {
            if (Collection.Any(y => y.Source == x.Source))
            {
                Warning(Language.ItemDuplicatedText);
                return;
            }

            Collection.Add(x);
            Detail!.Items.Add(x);
            Selected ??= Collection.FirstOrDefault();
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


        protected override void OnInitialize(ICollection<Album> collection)
        {
            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOfAlbum为空");
                return;
            }

            collection.AddRange(Detail.Items);
        }

        protected override void UpdateCommandState()
        {
            OpenCommand.NotifyCanExecuteChanged();
        }

        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Album> OpenCommand { get; }
    }
}