﻿using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlock : IModuleBlock, IModuleBlock<string>
    {
        /// <summary>
        /// 选项
        /// </summary>
        List<OptionItem> Items { get; }
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlockDataUI : IMonoSelectorBlock, IModuleBlockDataUI
    {
    }

    /// <summary>
    /// 表示多行内容块。
    /// </summary>
    public interface IMonoSelectorBlockEditUI : IModuleBlockEditUI, IModuleBlockEditUI<string>
    {
        /// <summary>
        /// 选项
        /// </summary>
        ObservableCollection<OptionItem> Items { get; }
    }
    
    public abstract class MonoSelectorBlock : ModuleBlock, IMonoSelectorBlock
    {
        public override void ClearValue()
        {
            Value = string.Empty;
        }
        
        protected sealed override bool CompareTemplateOverride(ModuleBlock block)
        {
            var mono = (MonoSelectorBlock)block;
            return Fallback == mono.Fallback &&
                   Items.SequenceEqual(mono.Items);
        }

        protected sealed override bool CompareValueOverride(ModuleBlock block)
        {
            var mono = (MonoSelectorBlock)block;
            return Value == mono.Value;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Fallback { get; init; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 选项
        /// </summary>
        public List<OptionItem> Items { get; init; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class RadioBlock : MonoSelectorBlock
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SequenceBlock : MonoSelectorBlock
    {
        
    }
}