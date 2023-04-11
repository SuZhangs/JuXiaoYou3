

using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Services;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class AbilityDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<AbilityBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<DetailPartView>(collection, "text.DocumentEditor.Detail");
            AddSubView<DataPartView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<ShareView>(collection, "text.DocumentEditor.Presentation");
        }

        protected override void OnCreateDocument(Document document)
        {
            document.Parts.Add(new PartOfAlbum{Items = new List<Album>()});
        }

        protected override void IsDataPartExistence(Document document)
        {
            
        }
    }
}