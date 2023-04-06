using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Controls.Models;
// ReSharper disable IdentifierTypo

namespace Acorisoft.FutureGL.MigaStudio.Models.Previews
{
    public abstract class PreviewBlockDataUI : ObservableObject
    {
        public static PreviewBlockDataUI GetDataUI(IPreviewBlockData value)
        {
            return value switch
            {
                PreviewTextData ptd => new PreviewBlockTextDataUI(ptd),
                PreviewDegreeData pdd => new PreviewBlockDegreeDataUI(pdd),
                PreviewStarData psd1 => new PreviewBlockStarDataUI(psd1),
                PreviewSwitchData psd2 => new PreviewBlockSwitchDataUI(psd2),
                PreviewHeartData phd => new PreviewBlockHeartDataUI(phd),
                PreviewProgressData ppd => new PreviewBlockProgressDataUI(ppd),
                PreviewRateData  prd => new PreviewBlockRateDataUI(prd),
                _ => throw new InvalidOperationException("没有这种数据")
            };
        }

        public virtual void Update(MetadataCollection metadataCollection)
        {
            
        }
        
        public string Metadata { get; protected init; }
        public string Name { get; init; }
    }

    public sealed class PreviewBlockStarDataUI : PreviewBlockDataUI
    {
        private bool _value;
        
        public PreviewBlockStarDataUI(IPreviewBlockData value)
        {
            Name     = value.Name;
            Metadata = value.Metadata;

        }

        public override void Update(MetadataCollection metadataCollection)
        {
            var v = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                              ?.Value;
            Value = bool.TryParse(v, out var n) && n;
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PreviewBlockSwitchDataUI : PreviewBlockDataUI
    {
        private bool _value;
        
        public PreviewBlockSwitchDataUI(PreviewSwitchData value)
        {
            Name     = value.Name;
            Metadata = value.Metadata;
        }


        public override void Update(MetadataCollection metadataCollection)
        {
            var v = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                      ?.Value;
            Value = bool.TryParse(v, out var n) && n;
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PreviewBlockRateDataUI : PreviewBlockDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PreviewBlockRateDataUI(IPreviewBlockData value)
        {
            Name     = value.Name;
            Metadata = value.Metadata;
        }
        public override void Update(MetadataCollection metadataCollection)
        {
            if (metadataCollection is null)
            {
                return;
            }
            
            var unParsedValue = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                                  ?.Value;
            MetadataValue = int.TryParse(unParsedValue, out var n) ? n : 0;
            Value         = MetadataValue / 5;
        }

        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public sealed class PreviewBlockDegreeDataUI : PreviewBlockDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PreviewBlockDegreeDataUI(IPreviewBlockData value)
        {
            Name          = value.Name;
            Metadata      = value.Metadata;
        }
        
        
        public override void Update(MetadataCollection metadataCollection)
        {
            if (metadataCollection is null)
            {
                return;
            }
            
            var unParsedValue = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                                  ?.Value;
            MetadataValue = int.TryParse(unParsedValue, out var n) ? n : 0;
            Value         = MetadataValue / 5;
        }
        
        
        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PreviewBlockProgressDataUI : PreviewBlockDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PreviewBlockProgressDataUI(IPreviewBlockData value)
        {
            Name          = value.Name;
            Metadata      = value.Metadata;
        }

        public override void Update(MetadataCollection metadataCollection)
        {
            if (metadataCollection is null)
            {
                return;
            }
            
            var unParsedValue = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                                  ?.Value;
            MetadataValue = int.TryParse(unParsedValue, out var n) ? n : 0;
            Value         = MetadataValue / 5;
        }

        /// <summary>
        /// 获取或设置 <see cref="MetadataValue"/> 属性。
        /// </summary>
        public int MetadataValue
        {
            get => _metadataValue;
            set => SetValue(ref _metadataValue, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public int Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
    
    public sealed class PreviewBlockTextDataUI : PreviewBlockDataUI
    {
        private string _value;
        
        public PreviewBlockTextDataUI(IPreviewBlockData value)
        {
            Name     = value.Name;
            Metadata = value.Metadata;
        }

        public override void Update(MetadataCollection metadataCollection)
        {
            if (metadataCollection is null)
            {
                return;
            }
            
            Value = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                      ?.Value;
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public sealed class PreviewBlockHeartDataUI : PreviewBlockDataUI
    {
        private bool _value;
        
        public PreviewBlockHeartDataUI(IPreviewBlockData value)
        {
            Name     = value.Name;
            Metadata = value.Metadata;
        }


        public override void Update(MetadataCollection metadataCollection)
        {
            var v = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                      ?.Value;
            Value = bool.TryParse(v, out var n) && n;
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public bool Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }
}