using System.Collections.Generic;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterDocumentViewModel : DocumentEditorVMBase
    {
        protected override void CreateSubViews(ICollection<HeaderedSubView> collection)
        {
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Basic");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.DataPart");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Detail");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Inspiration");
            AddSubView<CharacterBasicView>(collection, "text.DocumentEditor.Preview");
        }
        
        
    }
}