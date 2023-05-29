using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Share;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        public CharacterDocumentViewModel() : base()
        {
            SetAsMainPictureOfSquareCommand = AsyncCommand(SetAsMainPictureOfSquareImpl);
            SetAsMainPictureOfHorizontalCommand = AsyncCommand(SetAsMainPictureOfHorizontalImpl);
            SetAsMainPictureOfVerticalCommand = AsyncCommand(SetAsMainPictureOfVerticalImpl);
        }
        
        // TODO: 人物关系中的血缘关系
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddBasicView<CharacterBasicView>(collection);
            AddDetailView(collection);
            AddPartView(collection);
            AddShareView(collection);
            Preshapes.Add(new CharacterPergamynStyleView
            {
                Tag = Language.GetText("preshape.character.Pergamyn")
            });
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

        private void AddAlbumToDetailPart(Album album)
        {
            if (album is null)
            {
                return;
            }
            
            var part = DetailParts.OfType<PartOfAlbum>()
                                  .FirstOrDefault();

            if (part is null)
            {
                return;
            }

            if (part.Items.Any(x => x.Source == album.Source))
            {
                return;
            }
            
            part.Items.Add(album);
        }
        
        private void RemoveAlbumFromDetailPart(string album)
        {
            if (album is null)
            {
                return;
            }
            
            var part = DetailParts.OfType<PartOfAlbum>()
                                  .FirstOrDefault();

            if (part is null)
            {
                return;
            }

            var item = part.Items.FirstOrDefault(x => x.Source == album);

            if (item is null)
            {
                return;
            }
            
            part.Items
                .Remove(item);
        }

        private async Task SetAsMainPictureOfVerticalImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            if (!string.IsNullOrEmpty(MainPictureOfVertical))
            {
                //
                // delete
                RemoveAlbumFromDetailPart(MainPictureOfVertical);
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Vertical, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfVertical = r.Value;
            }
        }
        
        private async Task SetAsMainPictureOfHorizontalImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Horizontal, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfHorizontal = r.Value;
            }
        }
        
        private async Task SetAsMainPictureOfSquareImpl()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var r = await ImageUtilities.Raw(ImageEngine, opendlg.FileName, ImageUtilities.ImageScale.Square, AddAlbumToDetailPart);
            if (r.IsFinished)
            {
                MainPictureOfSquare = r.Value;
            }
        }

        public AsyncRelayCommand SetAsMainPictureOfVerticalCommand { get; }
        public AsyncRelayCommand SetAsMainPictureOfHorizontalCommand { get; }
        public AsyncRelayCommand SetAsMainPictureOfSquareCommand { get; }
    }
}