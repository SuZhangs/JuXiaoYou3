using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ColorServiceViewModel : TabViewModel
    {
        private ColorMapping _selected;
        private string       _color;
        private string       _selectedKeyword;

        public ColorServiceViewModel()
        {
            ColorService     = Xaml.Get<ColorService>();
            Property = Database.Get<ColorServiceProperty>();
            Keywords = new ObservableCollection<string>();
            Mappings = new ObservableCollection<ColorMapping>();

            if (Property.Mappings.Count > 0)
            {
                Mappings.AddRange(Property.Mappings, true);
            }

            AddMappingCommand    = AsyncCommand(AddMappingImpl);
            RemoveMappingCommand = AsyncCommand<ColorMapping>(RemoveMappingImpl, HasItem);
            EditMappingCommand   = AsyncCommand<ColorMapping>(EditMappingImpl, HasItem);
            
            AddKeywordCommand    = AsyncCommand<ColorMapping>(AddKeywordImpl, HasItem);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl,x => Selected is not null && HasItem(x));
            EditKeywordCommand   = AsyncCommand<string>(EditKeywordImpl, x => Selected is not null && HasItem(x));
            SaveCommand          = Command(SavePropertyImpl);
        }

        private void SavePropertyImpl()
        {
            Database.Set(Property);
            SetDirtyState(false);
        }

        private async Task AddMappingImpl()
        {
            var r = await StringViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            var m = new ColorMapping
            {
                Id       = ID.Get(),
                Name     = r.Value,
                Color    = "#007ACC",
                Keywords = new ObservableCollection<string> { r.Value },
            };

            Mappings.Add(m);
            Property.Mappings
                    .Add(m);
            ColorService.AddOrUpdate(r.Value, m.Color);
            SetDirtyState();
        }

        private async Task RemoveMappingImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Mappings.Remove(mapping);
            Property.Mappings
                    .Remove(mapping);
            SetDirtyState();

            if (mapping.Keywords.Count > 0)
            {
                foreach (var keyword in mapping.Keywords)
                {
                    ColorService.Remove(keyword);
                }
            }
        }

        private async Task EditMappingImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            var r = await StringViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            mapping.Name = r.Value;
            SetDirtyState();
        }

        private async Task AddKeywordImpl(ColorMapping mapping)
        {
            if (mapping is null)
            {
                return;
            }

            var r = await StringViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            var k = r.Value;

            if (string.IsNullOrEmpty(k) ||
                mapping.Keywords.Contains(k))
            {
                return;
            }

            mapping.Keywords.Add(k);
            Keywords.Add(k);
            SetDirtyState();

            if (!string.IsNullOrEmpty(mapping.Color))
            {
                ColorService.AddOrUpdate(k, mapping.Color);
            }
        }
        
        private async Task EditKeywordImpl(string keyword)
        {
            if (Selected is null || string.IsNullOrEmpty(keyword))
            {
                return;
            }

            var r = await StringViewModel.String(SubSystemString.EditNameTitle);
            if (!r.IsFinished)
            {
                return;
            }

            var k = r.Value;

            if (string.IsNullOrEmpty(k) ||
                Selected.Keywords.Contains(k))
            {
                return;
            }

            Selected.Keywords.Remove(keyword);
            Selected.Keywords.Add(k);
            ColorService.Remove(keyword);
            ColorService.AddOrUpdate(k, Selected.Color);
            Keywords.Remove(keyword);
            Keywords.Add(k);
            SetDirtyState();
        }
        
        private async Task RemoveKeywordImpl(string keyword)
        {
            if (Selected is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(keyword))
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            
            Selected.Keywords.Remove(keyword);
            ColorService.Remove(keyword);
            Keywords.Remove(keyword);
            SetDirtyState();
        }

        public IDatabase Database => DatabaseUtilities.Database;
        public ColorService ColorService { get; }
        public ColorServiceProperty Property { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedKeyword"/> 属性。
        /// </summary>
        public string SelectedKeyword
        {
            get => _selectedKeyword;
            set { 
                SetValue(ref _selectedKeyword, value);

                AddKeywordCommand.NotifyCanExecuteChanged();
                EditKeywordCommand.NotifyCanExecuteChanged();
                RemoveKeywordCommand.NotifyCanExecuteChanged();
            }
        }
        /// <summary>
        /// 获取或设置 <see cref="Color"/> 属性。
        /// </summary>
        public string Color
        {
            get => _color;
            set
            {
                SetValue(ref _color, value);

                if (Selected is not null)
                {
                    SetDirtyState();
                    Selected.Color = value;
                    ColorService.Changed(Selected.Keywords, value);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public ColorMapping Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);

                AddKeywordCommand.NotifyCanExecuteChanged();
                EditKeywordCommand.NotifyCanExecuteChanged();
                RemoveKeywordCommand.NotifyCanExecuteChanged();
                EditMappingCommand.NotifyCanExecuteChanged();
                RemoveMappingCommand.NotifyCanExecuteChanged();
                Keywords.Clear();
                if (value is not null)
                {
                    _color = value.Color;
                    RaiseUpdated(nameof(Color));
                    Keywords.AddRange(value.Keywords);
                }
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<string> Keywords { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ColorMapping> Mappings { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> EditMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> RemoveMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> EditKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}