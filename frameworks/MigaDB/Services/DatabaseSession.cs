using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class DatabaseSession
    {
        public bool DebugMode { get; init; }
        public IDatabase Database { get; init; }
        public string RootDirectory { get; init; }
    }
}