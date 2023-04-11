﻿namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class OtherDocumentViewModel: DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<OtherBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation");
        }

        protected override IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                new PartOfAlbum { Items    = new List<Album>() },
                new PartOfPlaylist { Items = new List<Music>() },
            };
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{ Items     = new List<Album>()});
            document.Parts.Add(new PartOfPlaylist { Items = new List<Music>() });
        }

        protected override void IsDataPartExistence(Document document)
        {
            
        }
    }
}