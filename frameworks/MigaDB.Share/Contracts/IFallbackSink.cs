namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IFallbackSink<T>
    {
        public T Fallback { get; set; }
    }
}