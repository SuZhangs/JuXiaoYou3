using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public abstract class ConstraintEditorVMBase : TabViewModel
    {
        public DocumentType Type { get; private set; }
    }
}