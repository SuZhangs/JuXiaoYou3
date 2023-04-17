using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Core;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ColorServiceViewModel : TabViewModel
    {
        private ColorMapping _selected;
        private string       _color;

        public ColorServiceViewModel()
        {
            ColorService = Xaml.Get<ColorService>();
            Database = Xaml.Get<IDatabaseManager>()
                           .Database
                           .CurrentValue;
            Property = Database.Get<ColorServiceProperty>();
            Keywords = new ObservableCollection<string>();
            Mappings = new ObservableCollection<ColorMapping>();

            AddMappingCommand    = AsyncCommand(AddMappingImpl);
            RemoveMappingCommand = AsyncCommand<ColorMapping>(RemoveMappingImpl, HasItem);
            EditMappingCommand   = AsyncCommand<ColorMapping>(EditMappingImpl, HasItem);
            
            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl,x => Selected is not null && HasItem(x));
            EditKeywordCommand   = AsyncCommand<string>(EditKeywordImpl, x => Selected is not null && HasItem(x));
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
                Keywords = new ObservableCollection<string>(),
            };

            Mappings.Add(m);
            Property.Mappings
                    .Add(m);
            Database.Upsert(Property);
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
            Database.Upsert(Property);

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
            Database.Upsert(Property);
        }

        private async Task AddKeywordImpl()
        {
            if (Selected is null)
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

            Selected.Keywords.Add(k);
            Database.Upsert(Property);

            if (!string.IsNullOrEmpty(Selected.Color))
            {
                ColorService.AddOrUpdate(k, Selected.Color);
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
            Database.Upsert(Property);
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
            Database.Upsert(Property);
        }

        public IDatabase Database { get; }
        public ColorService ColorService { get; }
        public ColorServiceProperty Property { get; }

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

                if (value is not null)
                {
                    _color = value.Color;
                    RaiseUpdated(nameof(Color));
                }
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<string> Keywords { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ColorMapping> Mappings { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> EditMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ColorMapping> RemoveMappingCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> EditKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}