using System.Collections.ObjectModel;
using System.Windows.Forms;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Models;

namespace Acorisoft.FutureGL.MigaStudio.Models.Previews
{
    public class PreviewBlockUI : StorageUIObject
    {
        private readonly PreviewBlock _source;

        public string Name { get; private init; }

        public PreviewBlock Source
        {
            get => _source;
            init
            {
                NameLists = new ObservableCollection<string>();
                DataLists = new ObservableCollection<PreviewBlockDataUI>();

                _source = value;
                Id      = value.Id;
                Name    = value.Name;
                
                foreach (var data in _source.DataLists)
                {
                    NameLists.Add(data.Name);
                    DataLists.Add(PreviewBlockDataUI.GetDataUI(data));
                }
            }
        }

        public ObservableCollection<string> NameLists { get; private init; }
        public ObservableCollection<PreviewBlockDataUI> DataLists { get; private init; }
    }

}