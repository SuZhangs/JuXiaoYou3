using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public class ComposeEditorViewModel : ComposeEditorBase
    {
        protected override void IsDataPartExistence(Compose document)
        {
            
        }

        protected override void OnCreateCompose(Compose document)
        {
        }

        protected override void OnStart()
        {
            this.Obsoleted("写作功能尚未完全开发完成", 10);
            base.OnStart();
        }
    }
}