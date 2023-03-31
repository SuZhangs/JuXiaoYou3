using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class DatabaseCreationMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            var timeOfBoth = DateTime.Now;
            
            database.Set<ModuleManifestProperty>(new ModuleManifestProperty
            {
                Manifests = new ObservableCollection<ModuleManifest>(),
                DefaultManifests = new Dictionary<DocumentType, ModuleManifest>(),
                DefaultPreviews = new Dictionary<DocumentType, PreviewManifest>()
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