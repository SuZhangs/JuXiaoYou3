namespace Acorisoft.FutureGL.MigaDB.Services
{
    public interface IDataEngine
    {
        bool Activate();
        bool Activated { get; }
        bool IsLazyMode { get; set; }
    }
}