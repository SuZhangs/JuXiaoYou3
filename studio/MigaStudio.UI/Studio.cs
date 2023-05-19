using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Services;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class Studio
    {
        private static readonly Lazy<IDatabaseManager> _databaseField = new Lazy<IDatabaseManager>(Xaml.Get<IDatabaseManager>);


        public static IDatabaseManager DatabaseManager() => _databaseField.Value;

        public static T This<T>() where T : class => _databaseField.Value
                                                                   .Database
                                                                   .CurrentValue
                                                                   .Get<T>();

        public static IDatabase Database() => _databaseField.Value
                                                            .Database
                                                            .CurrentValue;

        public static T ThisEngine<T>() where T : DataEngine => _databaseField.Value.GetEngine<T>();
    }
}