using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using DynamicData;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Acorisoft.FutureGL.MigaStudio.Modules
{
    public abstract class MonoSelectorDataUI : ModuleBlockDataUI<MonoSelectorBlock, string>, IMonoSelectorBlockDataUI
    {
        protected MonoSelectorDataUI(MonoSelectorBlock block) : base(block)
        {
            Items          = new List<OptionItemUI>();
            
            if (block.Items is not null && block.Items.Count > 0)
            {
                var id = string.IsNullOrEmpty(Id) ? GetHashCode().ToString() : Id;
                Items.AddRange(block.Items.Select(x => new OptionItemUI(id, Fallback, x, OnSelected)));
            }

            Value = string.IsNullOrEmpty(block.Value) ? block.Fallback : block.Value;
        }

        private void OnSelected(OptionItemUI item)
        {
            Value = item.Value;
        }
        
        protected override string OnValueChanged(string oldValue, string newValue)
        {
            if (Items is null)
            {
                return newValue;
            }
            
            if (Items.Any(x => x.Value != newValue))
            {
                newValue = Items.First().Value;
            }

            TargetBlock.Value = newValue;
            return newValue;

        }

        public List<OptionItemUI> Items { get; }
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

    
    public sealed class SequenceBlockDataUI : MonoSelectorDataUI
    {
        public SequenceBlockDataUI(SequenceBlock block) : base(block)
        {
        }
    }

    public sealed class SequenceBlockEditUI : MonoSelectorEditUI
    {
        public SequenceBlockEditUI(SequenceBlock block) : base(block)
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