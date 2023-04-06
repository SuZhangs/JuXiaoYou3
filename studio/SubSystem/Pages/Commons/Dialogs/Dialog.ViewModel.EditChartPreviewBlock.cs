using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs
{
    public class EditChartPreviewBlockViewModel : DialogViewModel
    {
        public static Task<Op> Edit(HistogramPreviewBlock hb, MetadataCollection documentMetas)
        {
            throw new NotImplementedException();
        }
        
        public static Task<Op> Edit(RadarPreviewBlock hb, MetadataCollection documentMetas)
        {
            throw new NotImplementedException();
        }
    }
}