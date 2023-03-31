﻿using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class ModuleManifestProperty : ObservableObject
    {
        public ModuleManifest GetModuleManifest(DocumentType type)
        {
            return DefaultManifests.TryGetValue(type, out var manifest) ? manifest : null;
        }
        
        public PreviewManifest GetPreviewManifest(DocumentType type)
        {
            return DefaultPreviews.TryGetValue(type, out var manifest) ? manifest : null;
        }

        public Dictionary<DocumentType, ModuleManifest> DefaultManifests { get; init; }
        public Dictionary<DocumentType, PreviewManifest> DefaultPreviews { get; init; }
        public ObservableCollection<ModuleManifest> Manifests { get; init; }
    }
}