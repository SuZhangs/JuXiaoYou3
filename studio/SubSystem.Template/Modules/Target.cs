using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class TargetBlockDataUI : ModuleBlockDataUI, ITargetBlockDataUI
    {
        protected TargetBlockDataUI(TargetBlock block) : base(block)
        {
            TargetBlock     = block;
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
        }
        
        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        private string _targetName;
        private string _targetSource;
        private string _targetThumbnail;

        /// <summary>
        /// 目标内容块
        /// </summary>
        protected TargetBlock TargetBlock { get; }

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set
            {
                SetValue(ref _targetThumbnail, value);
                TargetBlock.TargetThumbnail = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set
            {
                
                TargetBlock.TargetSource = value;
                SetValue(ref _targetSource, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set
            {
                TargetBlock.TargetName = value;
                SetValue(ref _targetName, value);
            }
        }
    }

    public class AudioBlockDataUI : TargetBlockDataUI, ITargetBlockDataUI
    {
        public AudioBlockDataUI(AudioBlock block) : base(block)
        {
        }
    }

    public class VideoBlockDataUI : TargetBlockDataUI, ITargetBlockDataUI
    {
        public VideoBlockDataUI(VideoBlock block) : base(block)
        {
        }
    }

    public class FileBlockDataUI : TargetBlockDataUI, ITargetBlockDataUI
    {
        public FileBlockDataUI(FileBlock block) : base(block)
        {
        }
    }

    public class MusicBlockDataUI : TargetBlockDataUI, ITargetBlockDataUI
    {
        public MusicBlockDataUI(MusicBlock block) : base(block)
        {
        }
    }

    public class ImageBlockDataUI : TargetBlockDataUI, ITargetBlockDataUI
    {
        public ImageBlockDataUI(ImageBlock block) : base(block)
        {
        }
    }

    public class ReferenceBlockDataUI : ModuleBlockDataUI, ITargetBlockDataUI, IReferenceBlock
    {
        public ReferenceBlockDataUI(ReferenceBlock block) : base(block)
        {
            TargetBlock     = block;
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
            DataSource      = block.DataSource;
        }
        
        public override bool CompareTemplate(ModuleBlock block)
        {
            return TargetBlock.CompareTemplate(block);
        }

        public override bool CompareValue(ModuleBlock block)
        {
            return TargetBlock.CompareValue(block);
        }

        private string          _targetName;
        private string          _targetSource;
        private string          _targetThumbnail;

        /// <summary>
        /// 目标内容块
        /// </summary>
        protected ReferenceBlock TargetBlock { get; }

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set
            {
                SetValue(ref _targetThumbnail, value);
                TargetBlock.TargetThumbnail = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set
            {
                
                TargetBlock.TargetSource = value;
                SetValue(ref _targetSource, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set
            {
                TargetBlock.TargetName = value;
                SetValue(ref _targetName, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="DataSource"/> 属性。
        /// </summary>
        public ReferenceSource DataSource { get; }
    }

    public class AudioBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public AudioBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new AudioBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class VideoBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public VideoBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new VideoBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class FileBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public FileBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override FileBlock CreateInstance()
        {
            return new FileBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class MusicBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public MusicBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new MusicBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class ImageBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public ImageBlockEditUI(AudioBlock block) : base(block)
        {
        }

        public override ModuleBlock CreateInstance()
        {
            return new ImageBlock
            {
                Id       = Id,
                Name     = Name,
                Metadata = Metadata,
                ToolTips = ToolTips,
            };
        }
    }

    public class ReferenceBlockEditUI : ModuleBlockEditUI, ITargetBlockEditUI
    {
        public ReferenceBlockEditUI(IReferenceBlock block) : base(block)
        {
            TargetName      = block.TargetName;
            TargetSource    = block.TargetSource;
            TargetThumbnail = block.TargetThumbnail;
            DataSource      = block.DataSource;
        }

        private ReferenceSource _dataSource;
        private string          _targetName;
        private string          _targetSource;
        private string          _targetThumbnail;

        /// <summary>
        /// 获取或设置 <see cref="TargetThumbnail"/> 属性。
        /// </summary>
        public string TargetThumbnail
        {
            get => _targetThumbnail;
            set => SetValue(ref _targetThumbnail, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetSource"/> 属性。
        /// </summary>
        public string TargetSource
        {
            get => _targetSource;
            set => SetValue(ref _targetSource, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TargetName"/> 属性。
        /// </summary>
        public string TargetName
        {
            get => _targetName;
            set => SetValue(ref _targetName, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="DataSource"/> 属性。
        /// </summary>
        public ReferenceSource DataSource
        {
            get => _dataSource;
            set => SetValue(ref _dataSource, value);
        }

        public override ModuleBlock CreateInstance()
        {
            return new ReferenceBlock()
            {
                Id         = Id,
                Name       = Name,
                Metadata   = Metadata,
                ToolTips   = ToolTips,
                DataSource = DataSource,
            };
        }
    }
}