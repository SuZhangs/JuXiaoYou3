using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Utils;

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
        
        public void SetPreviewManifest(DocumentType type, PartOfPreview manifest)
        {
            if (manifest is null)
            {
                return;
            }

            if (DefaultPreviewManifest.ContainsKey(type))
            {
                DefaultPreviewManifest[type] = manifest;
            }
            else
            {
                DefaultPreviewManifest.Add(type, manifest);
            }
        }

        public PartOfPreview GetPreviewManifest(DocumentType type, Action<ModuleManifestProperty> callback)
        {
            if (!DefaultPreviewManifest.TryGetValue(type, out var pop))
            {
                pop = new PartOfPreview
                {
                    Id     = ID.Get(),
                    Blocks = new ObservableCollection<PreviewBlock>()
                };
                DefaultPreviewManifest.Add(type, pop);
                callback?.Invoke(this);
            }

            return pop;
        }

        public Dictionary<DocumentType, string> DefaultManifests { get; init; }
        public Dictionary<DocumentType, PartOfPreview> DefaultPreviewManifest { get; init; }

        public ObservableCollection<ModuleManifest> Manifests { get; init; }
    }
}