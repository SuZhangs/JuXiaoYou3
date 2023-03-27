using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class DatabaseCreationMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            var timeOfBoth = DateTime.Now;
            
            database.Set<ModuleManifestProperty>(new ModuleManifestProperty
            {
                Manifests = new ObservableCollection<ModuleManifest>()
            });
            
            database.Set<DatabaseVersion>(new DatabaseVersion
            {
                TimeOfModified = timeOfBoth,
                TimeOfCreated  = timeOfBoth,
                Version        = Constants.MinVersion
            });
        }
    }
}