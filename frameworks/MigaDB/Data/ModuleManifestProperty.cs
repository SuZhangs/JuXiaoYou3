using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class ModuleManifestProperty : ObservableObject
    {
        public ModuleManifest GetModuleManifest(DocumentType type)
        {
            return DefaultManifests.ContainsKey(type) ? 
                Manifests.FirstOrDefault(x => x.Id == DefaultManifests[type]) :
                null;
        }
        
        public void SetModuleManifest(DocumentType type, ModuleManifest manifest)
        {
            if (manifest is null)
            {
                return;
            }

            if (DefaultManifests.ContainsKey(type))
            {
                DefaultManifests[type] = manifest.Id;
            }
            else
            {
                DefaultManifests.Add(type, manifest.Id);
            }
        }

        public Dictionary<DocumentType, string> DefaultManifests { get; init; }
        
        
        public ObservableCollection<ModuleManifest> Manifests { get; init; }
    }
}