using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class DatabaseUtilities
    {
        public static IDatabase Database => Xaml.Get<IDatabaseManager>()
                                                .Database
                                                .CurrentValue;
    }
}