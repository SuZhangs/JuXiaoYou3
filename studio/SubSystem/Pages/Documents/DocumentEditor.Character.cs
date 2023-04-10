using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<CharacterInspirationView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation");
        }
        
        
        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{ DataBags = new Dictionary<string, string>()});
            document.Parts.Add(new PartOfPlaylist{ DataBags = new Dictionary<string, string>()});
            document.Parts.Add(new PartOfRel());
        }

        protected override void IsDataPartExistence(Document document)
        {
        }
    }
}