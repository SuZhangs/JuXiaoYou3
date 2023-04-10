using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    public class DatabaseCreationMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            var timeOfBoth = DateTime.Now;

            database.Upsert<DatabaseProperty>(new DatabaseProperty
            {
                Author      = "Test",
                Name        = "Test",
                ForeignName = "Test"
            });

            database.Upsert<ModuleManifestProperty>(new ModuleManifestProperty
            {
                Manifests        = new ObservableCollection<ModuleManifest>(),
                DefaultManifests = new Dictionary<DocumentType, string>(),
                DefaultPresentationManifest = new Dictionary<DocumentType, PartOfPresentation>()
            });
            
            database.Upsert<DatabaseVersion>(new DatabaseVersion
            {
                TimeOfModified = timeOfBoth,
                TimeOfCreated  = timeOfBoth,
                Version        = Constants.MinVersion
            });
        }
    }
}