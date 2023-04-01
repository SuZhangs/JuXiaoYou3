using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class ModuleManifestViewModel : DialogViewModel
    {
        private ModuleManifest _selectedManifest;
        
        public ModuleManifestViewModel()
        {
            Property = Xaml.Get<IDatabaseManager>()
                           .Database
                           .CurrentValue
                           .Get<ModuleManifestProperty>();
        }

        private void Save()
        {
            Xaml.Get<IDatabaseManager>()
                .Database
                .CurrentValue
                .Set(Property);
        }

        private async Task AddManifestImpl()
        {
            var ds = DialogService();
            var r  = await StringViewModel.String(SubSystemString.EditNameTitle);

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await ds.Dialog<IEnumerable<ModuleTemplateCache>, ModuleSelectorViewModel>();
            if (!r1.IsFinished)
            {
                return;
            }

            var manifest = new ModuleManifest
            {
                Id        = ID.Get(),
                Name      = r.Value,
                Templates = new ObservableCollection<ModuleTemplateCache>(r1.Value)
            };
            
            Manifests.Add(manifest);
            Save();
        }
        
        private async Task RemoveManifestImpl(ModuleManifest manifest)
        {
            if (manifest is null)
            {
                return;
            }
            var ds = DialogService();
            var r  = await StringViewModel.String(SubSystemString.EditNameTitle);
            
            
            
            var r1 = await ds.Dialog<IEnumerable<ModuleTemplateCache>, ModuleSelectorViewModel>();
            if (!r1.IsFinished)
            {
                return;
            }
            
            Manifests.Add(manifest);
            Save();
        }
        
        public ModuleManifestProperty Property { get; }

        public ModuleManifest SelectedManifest
        {
            get => _selectedManifest;
            set => SetValue(ref _selectedManifest, value);
        }

        public ModuleManifest Ability
        {
            get => Property.GetModuleManifest(DocumentType.AbilityDocument);
            set
            {
                Property.SetModuleManifest(DocumentType.AbilityDocument, value);
                RaiseUpdated();
                Save();
            }
        }
        public ModuleManifest Character
        {
            get => Property.GetModuleManifest(DocumentType.CharacterDocument);
            set
            {
                Property.SetModuleManifest(DocumentType.CharacterDocument, value);
                RaiseUpdated();
                Save();
            }
        }
        public ModuleManifest Geography
        {
            get => Property.GetModuleManifest(DocumentType.GeographyDocument);
            set
            {
                Property.SetModuleManifest(DocumentType.GeographyDocument, value);
                RaiseUpdated();
                Save();
            }
        }
        
        public ModuleManifest Item
        {
            get => Property.GetModuleManifest(DocumentType.ItemDocument);
            set
            {
                Property.SetModuleManifest(DocumentType.ItemDocument, value);
                RaiseUpdated();
                Save();
            }
        }
        
        public ModuleManifest Other
        {
            get => Property.GetModuleManifest(DocumentType.OtherDocument);
            set
            {
                Property.SetModuleManifest(DocumentType.OtherDocument, value);
                RaiseUpdated();
                Save();
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleManifest> Manifests => Property.Manifests;
        
    }
}