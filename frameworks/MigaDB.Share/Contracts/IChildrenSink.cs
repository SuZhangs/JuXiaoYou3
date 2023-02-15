using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IChildrenSink<T>
    {
        T[] Items { get; set; }
    }
    
    public interface IBindableChildrenSink<T>
    {
        ObservableCollection<T> Items { get; set; }
    }
}