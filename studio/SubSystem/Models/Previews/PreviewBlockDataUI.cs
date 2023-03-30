using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaStudio.Models.Previews
{
    public abstract class PreviewBlockDataUI : ObservableObject
    {
        public static PreviewBlockDataUI GetDataUI(IPreviewBlockData data)
        {
            
        }
    }

    public sealed class PreviewBlockStarDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockSwitchDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockRateDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockLikabilityDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockDegreeDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockProgressDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockTextDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockRadarDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockHistogramDataUI : PreviewBlockDataUI
    {
        
    }
    
    public sealed class PreviewBlockHeartDataUI : PreviewBlockDataUI
    {
        
    }
}