using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {


        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DataPart> InvisibleDataParts { get; }


        [NullCheck(UniTestLifetime.Startup)]
        public ObservableCollection<Keyword> Keywords { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public DocumentType Type { get; private set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public Document Document { get; protected set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public DocumentCache Cache { get; protected set; }
    }
}