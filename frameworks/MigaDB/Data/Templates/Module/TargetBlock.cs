namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示目标内容块。
    /// </summary>
    public interface ITargetBlock : IModuleBlock
    {
        /// <summary>
        /// 数据的名字
        /// </summary>
        string TargetName { get; }

        /// <summary>
        /// 数据源
        /// </summary>
        string TargetSource { get; }

        /// <summary>
        /// 数据的缩略图
        /// </summary>
        string TargetThumbnail { get; }
    }

    /// <summary>
    /// 表示引用数据源。
    /// </summary>
    public enum ReferenceSource
    {
        /// <summary>
        /// 设定
        /// </summary>
        Rule,
        
        /// <summary>
        /// 剧情
        /// </summary>
        Compose,
        
        
    }
    
    /// <summary>
    /// 表示引用内容块。
    /// </summary>
    public interface IReferenceBlock : ITargetBlock
    {
        /// <summary>
        /// 数据来源
        /// </summary>
        ReferenceSource DataSource { get; }
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ITargetBlockDataUI : ITargetBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ITargetBlockEditUI : IModuleBlockEditUI
    {
    }

    public abstract class TargetBlock : ModuleBlock, ITargetBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block) => true;

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var tb = (TargetBlock)block;
            return TargetName == tb.TargetName &&
                   TargetSource == tb.TargetSource &&
                   TargetThumbnail == tb.TargetThumbnail;
        }

        public override void ClearValue()
        {
            TargetName      = string.Empty;
            TargetSource    = string.Empty;
            TargetThumbnail = string.Empty;
        }
        
        /// <summary>
        /// 数据的名字
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string TargetSource { get; set; }

        /// <summary>
        /// 数据的缩略图
        /// </summary>
        public string TargetThumbnail { get; set; }
    }

    public sealed class AudioBlock : TargetBlock
    {
        
    }
    
    public sealed class VideoBlock : TargetBlock
    {
        
    }
    
    public sealed class MusicBlock : TargetBlock
    {
        
    }
    
    public sealed class ImageBlock : TargetBlock
    {
        
    }
    
    public sealed class FileBlock : TargetBlock
    {
        
    }
    
    public sealed class ReferenceBlock : TargetBlock, IReferenceBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var rb = (ReferenceBlock)block;
            return DataSource == rb.DataSource;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var tb = (TargetBlock)block;
            return TargetName == tb.TargetName &&
                   TargetSource == tb.TargetSource &&
                   TargetThumbnail == tb.TargetThumbnail;
        }
        /// <summary>
        /// 数据来源
        /// </summary>
        public ReferenceSource DataSource { get; init; }
    }
}