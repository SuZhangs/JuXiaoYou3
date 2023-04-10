using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
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
        
        public void SetPresentationManifest(DocumentType type, PartOfPresentation manifest)
        {
            if (manifest is null)
            {
                return;
            }

            if (DefaultPresentationManifest.ContainsKey(type))
            {
                DefaultPresentationManifest[type] = manifest;
            }
            else
            {
                DefaultPresentationManifest.Add(type, manifest);
            }
        }

        public PartOfPresentation GetPresentationManifest(DocumentType type, Action<ModuleManifestProperty> callback)
        {
            if (!DefaultPresentationManifest.TryGetValue(type, out var pop))
            {
                pop = new PartOfPresentation
                {
                    Id     = ID.Get(),
                    Blocks = new ObservableCollection<Presentation>()
                };
                DefaultPresentationManifest.Add(type, pop);
                callback?.Invoke(this);
            }

            return pop;
        }

        public Dictionary<DocumentType, string> DefaultManifests { get; init; }
        public Dictionary<DocumentType, PartOfPresentation> DefaultPresentationManifest { get; init; }

        public ObservableCollection<ModuleManifest> Manifests { get; init; }
    }
}