using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        public CharacterDocumentViewModel() : base()
        {
            SetAsMainPictureOf4x3Command = AsyncCommand(SetAsMainPictureOf4x3Impl);
            SetAsMainPictureOf16x9Command = AsyncCommand(SetAsMainPictureOf16x9Impl);
            SetAsMainPictureOf9x16Command = AsyncCommand(SetAsMainPictureOf9x16Impl);
        }
        
        // TODO: 人物关系中的血缘关系
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddBasicView<CharacterBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                new PartOfAlbum { Items      = new List<Album>() },
                new PartOfPlaylist { Items   = new List<Music>() },
                new PartOfAppraise { Items   = new List<Appraise>() },
                new PartOfStickyNote { Items = new List<StickyNote>() },
                new PartOfPrototype { Items  = new List<Prototype>() },
                new PartOfSentence { Items   = new List<Sentence>() },
                new PartOfSurvey { Items     = new List<SurveySet>() },
                new PartOfRel(),
            };
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum { Items    = new List<Album>() });
            document.Parts.Add(new PartOfPlaylist { Items = new List<Music>() });
            document.Parts.Add(new PartOfRel());
        }

        protected override void IsDataPartExistence(Document document)
        {
        }

        private async Task SetAsMainPictureOf9x16Impl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, 9, 16);
            if (r.IsFinished)
            {
                MainPictureOf9x16 = r.Value;
            }
        }
        
        private async Task SetAsMainPictureOf16x9Impl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, 16, 9);
            if (r.IsFinished)
            {
                MainPictureOf16x9 = r.Value;
            }
        }
        
        private async Task SetAsMainPictureOf4x3Impl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, 4, 3);
            if (r.IsFinished)
            {
                MainPictureOf4x3 = r.Value;
            }
        }

        public AsyncRelayCommand SetAsMainPictureOf9x16Command { get; }
        public AsyncRelayCommand SetAsMainPictureOf16x9Command { get; }
        public AsyncRelayCommand SetAsMainPictureOf4x3Command { get; }
    }
}