﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using CommunityToolkit.Mvvm.Input;

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
            AddManifestCommand          = AsyncCommand(AddManifestImpl);
            RemoveManifestCommand       = AsyncCommand<ModuleManifest>(RemoveManifestImpl, x => x is not null);
            SetAbilityManifestCommand   = Command<ModuleManifest>(x => Ability   = x);
            SetCharacterManifestCommand = Command<ModuleManifest>(x => Character = x);
            SetGeographyManifestCommand = Command<ModuleManifest>(x => Geography = x);
            SetItemManifestCommand      = Command<ModuleManifest>(x => Item      = x);
            SetOtherManifestCommand     = Command<ModuleManifest>(x => Other     = x);
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

            var r1 = await ds.Dialog<IEnumerable<ModuleTemplateCache>, ModuleSelectorViewModel>(new RouteEventArgs
            {
                Args = new object[]
                {
                    Xaml.Get<IDatabaseManager>()
                        .GetEngine<TemplateEngine>()
                        .TemplateCacheDB
                        .FindAll()
                }
            });
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

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            Manifests.Remove(manifest);
            
            if (Property.DefaultManifests
                        .Values
                        .Any(x => ReferenceEquals(x, manifest)))
            {
                await Warning(Language.GetText("text.defaultManifestHasBeenRemoved"));
            }
            
            Save();
        }

        public ModuleManifestProperty Property { get; }

        public ModuleManifest SelectedManifest
        {
            get => _selectedManifest;
            set
            {
                SetValue(ref _selectedManifest, value);
                RemoveManifestCommand.NotifyCanExecuteChanged();
            }
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
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddManifestCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleManifest> RemoveManifestCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleManifest> SetAbilityManifestCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleManifest> SetCharacterManifestCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleManifest> SetGeographyManifestCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleManifest> SetItemManifestCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<ModuleManifest> SetOtherManifestCommand { get; }

    }
}