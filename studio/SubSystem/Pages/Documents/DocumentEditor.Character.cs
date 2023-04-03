using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Preview");
        }
        
        
        protected override void OnCreateDocument(Document document)
        {
            AddDetailPart(new PartOfRel());
            AddDetailPart(new PartOfAlbum());
            AddDetailPart(new PartOfPlaylist());
            document.Parts.Add(new PartOfRel());
            document.Parts.Add(new PartOfAlbum());
            document.Parts.Add(new PartOfPlaylist());
        }
    }
}