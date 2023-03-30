using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewBlock : StorageUIObject
    {
        public ObservableCollection<IPreviewBlockData> DataLists { get; init; }
    }
}