using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public abstract class DocumentEditorVMBase : TabViewModel
    {
        public DocumentType Type { get; private set; }
    }
}