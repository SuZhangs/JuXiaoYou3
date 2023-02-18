﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlock : IModuleBlock, IModuleBlock<int>
    {
        /// <summary>
        /// 分割线
        /// </summary>
        int DivideLine { get; }
        
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; }
        
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
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlockDataUI : IDegreeBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public interface IDegreeBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<int>
    {
        /// <summary>
        /// 分割线
        /// </summary>
        int DivideLine { get; set; }
        
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; set; }
        
        /// <summary>
        /// 最大值
        /// </summary>
        int Maximum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        int Minimum { get; set; }
    }
    
    /// <summary>
    /// 表示程度内容块。
    /// </summary>
    public class DegreeBlock : ModuleBlock, IDegreeBlock
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

        /// <summary>
        /// 后缀
        /// </summary>
        public int DivideLine { get; init; }
        
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Negative { get; init; }
        
        
        /// <summary>
        /// 后缀
        /// </summary>
        public string Positive { get; init; }
    }
}