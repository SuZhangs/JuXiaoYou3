using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Models
{
    public class ModuleManifestProperty : ObservableObject
    {
        private ModuleManifest _defaultCharacterManifest;
        private ModuleManifest _defaultAbilityManifest;
        private ModuleManifest _defaultGeometryManifest;
        private ModuleManifest _defaultItemManifest;
        private ModuleManifest _defaultOtherManifest;
        private ModuleManifest _defaultMysteryManifest;
        
        public ModuleManifest DefaultAbilityManifest
        {
            get => _defaultAbilityManifest;
            set => SetValue(ref _defaultAbilityManifest, value);
        }
        
        public ModuleManifest DefaultGeometryManifest
        {
            get => _defaultGeometryManifest;
            set => SetValue(ref _defaultGeometryManifest, value);
        }
        
        public ModuleManifest DefaultItemManifest
        {
            get => _defaultItemManifest;
            set => SetValue(ref _defaultItemManifest, value);
        }
        
        public ModuleManifest DefaultOtherManifest
        {
            get => _defaultOtherManifest;
            set => SetValue(ref _defaultOtherManifest, value);
        }
        
        public ModuleManifest DefaultMysteryManifest
        {
            get => _defaultMysteryManifest;
            set => SetValue(ref _defaultMysteryManifest, value);
        }
        

        public ModuleManifest DefaultCharacterManifest
        {
            get => _defaultCharacterManifest;
            set => SetValue(ref _defaultCharacterManifest, value);
        }
        
        public ObservableCollection<ModuleManifest> Manifests { get; init; }
    }
}