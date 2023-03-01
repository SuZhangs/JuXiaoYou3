using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaStudio.Models;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class SettingViewModelBase : TabViewModel
    {
        protected SettingViewModelBase()
        {
            Items = new ObservableCollection<ISettingItem>();
        }
        
        protected ISettingItem Text(string id, Action<string> callback)
        {
            var mainTitleKey = $"{id}";
            var subTitleKey = $"{id}.Tips";

            var item = new SettingField
            {
                MainTitle = Language.GetText(mainTitleKey),
                SubTitle = Language.GetText(subTitleKey),
                Callback = callback,
            };
            
            Items.Add(item);
            return item;
        }
        
        protected ISettingItem ComboBox<T>(string id, Action<T> callback, IEnumerable<object> collection)
        {
            var mainTitleKey = $"{id}";
            var subTitleKey = $"{id}.Tips";

            var item = new SettingComboBox<T>
            {
                MainTitle = Language.GetText(mainTitleKey),
                SubTitle = Language.GetText(subTitleKey),
                Callback = callback,
                Collection = collection
            };
            
            Items.Add(item);
            return item;
        }
        
        public ObservableCollection<ISettingItem> Items { get; }
    }
}