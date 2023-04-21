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
        public ObservableCollection<string> Keywords => Cache.Keywords;


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
        
        /// <summary>
        /// 
        /// </summary>
        public DocumentType Type { get; private set; }
        public Document Document { get; protected set; }
        public DocumentCache Cache { get; protected set; }
    }
}