namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IBindableSink<T>
    {
        T Value { get; set; }
    }
}