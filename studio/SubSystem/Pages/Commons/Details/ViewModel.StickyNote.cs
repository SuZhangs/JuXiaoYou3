using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Details
{
    public class StickyNotePartViewModel : DetailViewModel<PartOfStickyNote>
    {
        public StickyNotePartViewModel()
        {
            Collection       = new ObservableCollection<StickyNote>();
            AddCommand       = Command(AddImpl);
            RemoveCommand    = AsyncCommand<StickyNote>(RemoveImpl, HasItem);
            ShiftUpCommand   = Command<StickyNote>(ShiftUpImpl, HasItem);
            ShiftDownCommand = Command<StickyNote>(ShiftDownImpl, HasItem);
            OpenCommand      = AsyncCommand<StickyNote>(OpenImpl, HasItem);
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

        private void AddImpl()
        {
            Collection.Add(new StickyNote
            {
                Id            = ID.Get(),
                TimeOfCreated = DateTime.Now
            });
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
            Collection.ShiftDown(stickyNote);
            Save();
        }

        private void ShiftUpImpl(StickyNote stickyNote)
        {
            Collection.ShiftUp(stickyNote);
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
        public RelayCommand AddCommand { get; }

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