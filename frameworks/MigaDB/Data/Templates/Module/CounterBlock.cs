﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
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
        
    }
    
    public class RateBlock : CounterBlock
    {
        
    }
}