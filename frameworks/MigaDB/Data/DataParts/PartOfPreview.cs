using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPreview : PartOfManifest
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<PreviewBlock> Blocks { get; init; }
    }
}