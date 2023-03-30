﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlock : IModuleBlock, IModuleBlock<int>
    {
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; }
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlockDataUI : ICounterBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface ICounterBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<int>
    {
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; set; }
    }
    
    public abstract class CounterBlock : ModuleBlock, ICounterBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (CounterBlock)block;
            return Fallback == bb.Fallback &&
                   Maximum == bb.Maximum && 
                   Minimum == bb.Minimum;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (CounterBlock)block;
            return bb.Value == Value;
        }
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = -1;

        /// <summary>
        /// 当前值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public int Fallback { get; init; }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum { get; init; }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Minimum { get; init; }
    }

    public class LikabilityBlock : CounterBlock
    {
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Likability,
            };
        }
    }
    
    public class RateBlock : CounterBlock
    {
        
        public sealed override Metadata ExtractMetadata()
        {
            return new Metadata
            {
                Name  = Metadata,
                Value = Value.ToString(),
                Type  = MetadataKind.Rate,
            };
        }
    }
}