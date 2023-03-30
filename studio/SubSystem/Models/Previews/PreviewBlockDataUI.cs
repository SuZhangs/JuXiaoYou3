using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Controls.Models;

namespace Acorisoft.FutureGL.MigaStudio.Models.Previews
{
    public abstract class PreviewBlockDataUI : ObservableObject
    {
        public static PreviewBlockDataUI GetDataUI(IPreviewBlockData data)
        {
            return data switch
            {
                PreviewTextData ptd => new PreviewBlockTextDataUI(ptd),
                PreviewDegreeData pdd => new PreviewBlockDegreeDataUI(pdd),
                PreviewStarData psd1 => new PreviewBlockStarDataUI(psd1),
                PreviewSwitchData psd2 => new PreviewBlockSwitchDataUI(psd2),
                PreviewHeartData phd => new PreviewBlockHeartDataUI(phd),
                PreviewHistogramData phcd => new PreviewBlockHistogramDataUI(phcd),
                PreviewRadarData prcd => new PreviewBlockRadarDataUI(prcd),
                PreviewLikabilityData pld => new PreviewBlockLikabilityDataUI(pld),
                PreviewProgressData ppd => new PreviewBlockProgressDataUI(ppd),
                PreviewRateData  prd => new PreviewBlockRateDataUI(prd),
            };
        }
    }

    public sealed class PreviewBlockStarDataUI : PreviewBlockDataUI
    {
        public PreviewBlockStarDataUI(PreviewStarData psd1)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockSwitchDataUI : PreviewBlockDataUI
    {
        public PreviewBlockSwitchDataUI(PreviewSwitchData psd2)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockRateDataUI : PreviewBlockDataUI
    {
        public PreviewBlockRateDataUI(PreviewRateData prd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockLikabilityDataUI : PreviewBlockDataUI
    {
        public PreviewBlockLikabilityDataUI(PreviewLikabilityData pld)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockDegreeDataUI : PreviewBlockDataUI
    {
        public PreviewBlockDegreeDataUI(PreviewDegreeData pdd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockProgressDataUI : PreviewBlockDataUI
    {
        public PreviewBlockProgressDataUI(PreviewProgressData ppd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockTextDataUI : PreviewBlockDataUI
    {
        public PreviewBlockTextDataUI(PreviewTextData ptd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockRadarDataUI : PreviewBlockDataUI
    {
        public PreviewBlockRadarDataUI(PreviewRadarData prcd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockHistogramDataUI : PreviewBlockDataUI
    {
        public PreviewBlockHistogramDataUI(PreviewHistogramData phcd)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PreviewBlockHeartDataUI : PreviewBlockDataUI
    {
        public PreviewBlockHeartDataUI(PreviewHeartData phd)
        {
            throw new NotImplementedException();
        }
    }
}