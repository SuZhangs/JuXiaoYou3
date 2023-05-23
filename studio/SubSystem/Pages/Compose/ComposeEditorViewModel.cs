using System.Linq;
using Acorisoft.FutureGL.MigaDB;
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
            var db = Studio.Database();

#if DEBUG

            this.Obsoleted(Language.GetText(Feature.TextEditorFeatureMissing), 10);
#else
            if (!db.Boolean(Feature.TextEditorFeatureMissing))
            {
                this.Obsoleted(Language.GetText(Feature.TextEditorFeatureMissing), 10);

                db.Boolean(Feature.TextEditorFeatureMissing, true);
            }
#endif
            base.OnStart();
        }
    }
}