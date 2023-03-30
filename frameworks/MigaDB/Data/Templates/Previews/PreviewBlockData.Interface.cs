using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public interface IPreviewBlockData : INotifyPropertyChanged
    {
        string Name { get; }
    }
}