using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class StickyNotePartViewModelProxy : BindingProxy<StickyNotePartViewModel>
    {
    }

    public class StickyNotePartViewModel : DetailViewModel<PartOfStickyNote>
    {
        public StickyNotePartViewModel()
        {
            Collection       = new ObservableCollection<StickyNote>();
            AddCommand       = AsyncCommand(AddImpl);
            RemoveCommand    = AsyncCommand<StickyNote>(RemoveImpl);
            ShiftUpCommand   = Command<StickyNote>(ShiftUpImpl);
            ShiftDownCommand = Command<StickyNote>(ShiftDownImpl);
            OpenCommand      = AsyncCommand<StickyNote>(OpenImpl);
        }

        public override void Start()
        {
            base.Start();

            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOf为空");
                return;
            }

            Collection.AddRange(Detail.Items);
        }

        private async Task AddImpl()
        {
            var title = await StringViewModel.String(SubSystemString.EditNameTitle);

            if (!title.IsFinished)
            {
                return;
            }

            var now = DateTime.Now;
            var item = new StickyNote
            {
                Id             = ID.Get(),
                Title          = title.Value,
                TimeOfCreated  = now,
                TimeOfModified = now
            };
            Collection.Add(item);
            Detail.Items.Add(item);
        }

        private async Task RemoveImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Collection.Remove(stickyNote);
            Detail.Items.Remove(stickyNote);
            Save();
        }

        private async Task OpenImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            await EditPlainTextViewModel.Edit(stickyNote);
            Save();
        }

        private void ShiftDownImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            Collection.ShiftDown(stickyNote);
            Detail.Items.ShiftDown(stickyNote);
            Save();
        }

        private void ShiftUpImpl(StickyNote stickyNote)
        {
            if (stickyNote is null)
            {
                return;
            }

            Collection.ShiftUp(stickyNote);
            Detail.Items.ShiftUp(stickyNote);
            Save();
        }

        private void Save()
        {
            Owner.SetDirtyState();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<StickyNote> Collection { get; init; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<StickyNote> ShiftUpCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<StickyNote> ShiftDownCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<StickyNote> OpenCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<StickyNote> RemoveCommand { get; }
    }
}