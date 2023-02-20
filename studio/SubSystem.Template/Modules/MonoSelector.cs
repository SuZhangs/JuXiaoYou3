﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using DynamicData;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class MonoSelectorDataUI : ModuleBlockDataUI<MonoSelectorBlock, string>, IMonoSelectorBlockDataUI
    {
        protected MonoSelectorDataUI(MonoSelectorBlock block) : base(block)
        {
            Items          = new List<OptionItem>();
            
            if (block.Items is not null && block.Items.Count > 0)
            {
                Items.AddRange(block.Items);
            }
        }

        protected override string OnValueChanged(string oldValue, string newValue)
        {
            if (Items.Any(x => x.Value != newValue))
            {
                newValue = Items.First().Value;
            }

            TargetBlock.Value = newValue;
            return newValue;

        }

        public List<OptionItem> Items { get; }
    }

    public abstract class MonoSelectorEditUI : ModuleBlockEditUI<MonoSelectorBlock, string>, IMonoSelectorBlockEditUI
    {
        private bool _allowDiffValue;

        protected MonoSelectorEditUI(MonoSelectorBlock block) : base(block)
        {
            Items = new ObservableCollection<OptionItem>();
            
            if (block.Items is not null && block.Items.Count > 0)
            {
                Items.AddRange(block.Items);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="AllowDiffValue"/> 属性。
        /// </summary>
        public bool AllowDiffValue
        {
            get => _allowDiffValue;
            set => SetValue(ref _allowDiffValue, value);
        }

        public ObservableCollection<OptionItem> Items { get; }
    }

    public sealed class RadioBlockDataUI : MonoSelectorDataUI
    {
        public RadioBlockDataUI(RadioBlock block) : base(block)
        {
        }
    }
    
    public sealed class SequenceBlockDataUI : MonoSelectorDataUI
    {
        public SequenceBlockDataUI(SequenceBlock block) : base(block)
        {
        }
    }

    public sealed class RadioBlockEditUI : MonoSelectorEditUI
    {
        public RadioBlockEditUI(RadioBlock block) : base(block)
        {
        }

        protected override MonoSelectorBlock CreateInstanceOverride()
        {
            return new RadioBlock
            {
                Id       = Id,
                Name     = Name,
                Items    = new List<OptionItem>(Items),
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
            };
        }
    }
    
    public sealed class SequenceBlockEditUI : MonoSelectorEditUI
    {
        public SequenceBlockEditUI(RadioBlock block) : base(block)
        {
        }

        protected override MonoSelectorBlock CreateInstanceOverride()
        {
            return new SequenceBlock
            {
                Id       = Id,
                Name     = Name,
                Items    = new List<OptionItem>(Items),
                Metadata = Metadata,
                Fallback = Fallback,
                ToolTips = ToolTips,
            };
        }
    }
}