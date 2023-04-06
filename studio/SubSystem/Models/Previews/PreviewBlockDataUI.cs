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
                PreviewLikabilityData pld => new PreviewBlockLikabilityDataUI(pld),
                _ => throw new InvalidOperationException("没有这种数据")
            };
        }

        protected PreviewBlockDataUI(IPreviewBlockData data) => IsMetadata = data.IsMetadata;

        public virtual void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            
        }

        public bool GetBooleanValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            if (IsMetadata)
            {
                return bool.TryParse(metadataTracker(Metadata)?.Value, out var n) && n;
            }
            
            return ((IMetadataBooleanSource)blockTracker(Metadata))?.GetValue() ?? false;
        }
        
        public int GetNumberValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            if (IsMetadata)
            {
                return int.TryParse(metadataTracker(Metadata)?.Value, out var n) ? n : 0;
            }
            
            return ((IMetadataNumericSource)blockTracker(Metadata))?.GetValue() ?? 0;
        }
        
        public string GetStringValue(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            return IsMetadata ? metadataTracker(Metadata)?.Value : ((IMetadataTextSource)blockTracker(Metadata))?.GetValue();
        }
        
        public bool IsMetadata { get; }
        public string Metadata { get; protected init; }
        public string Name { get; init; }
    }

    public sealed class PreviewBlockStarDataUI : PreviewBlockDataUI
    {
        private bool _value;
        
        public PreviewBlockStarDataUI(IPreviewBlockData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;

        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
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
        
        public PreviewBlockSwitchDataUI(PreviewSwitchData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }


        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
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
        
        public PreviewBlockRateDataUI(IPreviewBlockData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
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
    
    
    public sealed class PreviewBlockLikabilityDataUI : PreviewBlockDataUI
    {
        private int _value;
        private int _metadataValue;
        
        public PreviewBlockLikabilityDataUI(IPreviewBlockData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
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
        
        public PreviewBlockDegreeDataUI(IPreviewBlockData value) : base(value)
        {
            Name          = value.Name;
            Metadata      = value.ValueSourceID;
        }
        
        
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
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
        
        public PreviewBlockProgressDataUI(IPreviewBlockData value) : base(value)
        {
            Name          = value.Name;
            Metadata      = value.ValueSourceID;
        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            MetadataValue = GetNumberValue(metadataTracker, blockTracker);
            Value         = MetadataValue > 100 ? MetadataValue / 100 : MetadataValue;
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
        
        public PreviewBlockTextDataUI(IPreviewBlockData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }

        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetStringValue(metadataTracker, blockTracker);
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
        
        public PreviewBlockHeartDataUI(IPreviewBlockData value) : base(value)
        {
            Name     = value.Name;
            Metadata = value.ValueSourceID;
        }


        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = GetBooleanValue(metadataTracker, blockTracker);
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