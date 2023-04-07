using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class EasyDocumentGalleryViewModel : TabViewModel
    {
        protected override void OnStart(Parameter parameter)
        {
            Type = (DocumentType)parameter.Args[2];
            base.OnStart(parameter);
        }
        
        public DocumentType Type { get; private set; }
    }
}