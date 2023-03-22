﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlock : IModuleBlock, IModuleBlock<bool>
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlockDataUI : IOptionBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public interface IOptionBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<bool>
    {
    }
    
    /// <summary>
    /// 表示选项内容块。
    /// </summary>
    public abstract class OptionBlock : ModuleBlock,IOptionBlock
    {
        protected override bool CompareTemplateOverride(ModuleBlock block)
        {
            var bb = (OptionBlock)block;
            return Fallback == bb.Fallback;
        }

        protected override bool CompareValueOverride(ModuleBlock block)
        {
            var bb = (OptionBlock)block;
            return bb.Value == Value;
        }
        
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
    }

    /// <summary>
    /// 表示开关内容块。
    /// </summary>
    public sealed class SwitchBlock : OptionBlock
    {
        
    }
    
    /// <summary>
    /// 表示红心内容块。原来的Favorite
    /// </summary>
    public sealed class HeartBlock : OptionBlock
    {
        
    }
    
    
    /// <summary>
    /// 表示星星内容块。原来的Talent
    /// </summary>
    public sealed class StarBlock : OptionBlock
    {
        
    }
}