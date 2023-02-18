﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{    
    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlock : IModuleBlock, IModuleBlock<bool>
    {
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; }
    }

    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlockDataUI : IBinaryBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public interface IBinaryBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>
    {
        /// <summary>
        /// 负面值
        /// </summary>
        string Negative { get; set; }
        
        /// <summary>
        /// 正面值
        /// </summary>
        string Positive { get; set; }
    }
    
    
    /// <summary>
    /// 表示两极内容块。
    /// </summary>
    public class BinaryBlock : ModuleBlock, IBinaryBlock
    {
        /// <summary>
        /// 清除当前值。
        /// </summary>
        public override void ClearValue() => Value = false;
        
        /// <summary>
        /// 当前值
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public bool Fallback { get; init; }

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