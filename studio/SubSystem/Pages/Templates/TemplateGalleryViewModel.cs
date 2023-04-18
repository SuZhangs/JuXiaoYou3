﻿using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.Miga.Doc.Parts;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;
using NLog;
using Ookii.Dialogs.Wpf;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class TemplateGalleryViewModel : TabViewModel, IPresentationViewModel
    {
        private const string FileNotExists = "text.FileNotFound";

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly ReadOnlyObservableCollection<ModuleTemplateCache> _collection;

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly BehaviorSubject<Func<ModuleTemplateCache, bool>> _sorter;

        [NullCheck(UniTestLifetime.Constructor)]
        private readonly DataPartReader _reader;

        private DocumentType        _type;
        private ModuleTemplateCache _selectedTemplate;
        private bool                _isPresentation;

        public TemplateGalleryViewModel()
        {
            TemplateEngine = Xaml.Get<IDatabaseManager>()
                                 .GetEngine<TemplateEngine>();

            Property = Xaml.Get<IDatabaseManager>()
                           .Database
                           .CurrentValue
                           .Get<ModuleManifestProperty>();

            _sorter                     = new BehaviorSubject<Func<ModuleTemplateCache, bool>>(Xaml.AlwaysTrue);
            _reader                     = new DataPartReader();
            Source                      = new SourceList<ModuleTemplateCache>();
            MetadataCollection          = new ObservableCollection<MetadataCache>();
            Blocks                      = new ObservableCollection<ModuleBlock>();
            ApprovalRequired            = false;
            ManageManifestCommand       = AsyncCommand(ManageManifestImpl);
            AddTemplateCommand          = AsyncCommand(AddTemplateImpl);
            ImportTemplateCommand       = AsyncCommand(ImportTemplateImpl);
            ExportTemplateCommand       = AsyncCommand<FrameworkElement>(ExportTemplateImpl, x => x is not null && SelectedTemplate is not null);
            RemoveTemplateCommand       = AsyncCommand<ModuleTemplateCache>(RemoveTemplateImpl);
            PresentationCommand         = Command(() => IsPresentation       = true, () => SelectedTemplate is not null);
            SetAbilityManifestCommand   = AsyncCommand(async () => Ability   = await PickModuleManifestImpl());
            SetCharacterManifestCommand = AsyncCommand(async () => Character = await PickModuleManifestImpl());
            SetGeographyManifestCommand = AsyncCommand(async () => Geography = await PickModuleManifestImpl());
            SetItemManifestCommand      = AsyncCommand(async () => Item      = await PickModuleManifestImpl());
            SetOtherManifestCommand     = AsyncCommand(async () => Other     = await PickModuleManifestImpl());

            Source.Connect()
                  .Filter(_sorter)
                  .ObserveOn(Scheduler)
                  .Bind(out _collection)
                  .Subscribe()
                  .DisposeWith(Collector);
        }

        public override void OnStart()
        {
            TemplateEngine.Activate();
            Refresh();
        }


        private void Save()
        {
            Xaml.Get<IDatabaseManager>()
                .Database
                .CurrentValue
                .Set(Property);
        }

        private async Task AddTemplateImpl()
        {
            var opendlg = FileIO.Open(SubSystemString.ModuleFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            //
            // 
            var fileNames     = opendlg.FileNames;
            var finishedCount = 0;
            var errorCount    = 0;

            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                {
                    Error(string.Format(Language.GetText(FileNotExists), fileName));
                    continue;
                }

                if (!TemplateEngine.Activated)
                {
                    TemplateEngine.Activate();
                }

                var r = await TemplateEngine.AddModule(fileName);

                if (!r.IsFinished)
                {
                    errorCount++;
                }
                else
                {
                    finishedCount++;
                }
            }

            if (finishedCount == 0)
            {
                Error(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                Successful(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
            }

            Refresh();
        }

        private async Task RemoveTemplateImpl(ModuleTemplateCache item)
        {
            if (item is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            var logger = Xaml.Get<ILogger>();
            var r      = TemplateEngine.RemoveModule(item);

            if (!r.IsFinished)
            {
                var msg = Language.GetEnum(r.Reason);

                logger.Warn(msg);
                await Warning(msg);
            }
            else
            {
                if (item == SelectedTemplate)
                {
                    SelectedTemplate = null;
                }

                Refresh();
            }
        }

        private async Task ImportTemplateImpl()
        {
            var opendlg = FileIO.Open(SubSystemString.ModuleFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var logger        = Xaml.Get<ILogger>();
            var finishedCount = 0;
            var errorCount    = 0;

            foreach (var fileName in opendlg.FileNames)
            {
                try
                {
                    var result      = await _reader.ReadFromAsync(fileName);
                    var oldTemplate = result.Result;
                    var template    = ModuleBlockFactory.Upgrade(oldTemplate);
                    var r           = TemplateEngine.AddModule(template);

                    if (!r.IsFinished)
                    {
                        errorCount++;
                    }
                    else
                    {
                        finishedCount++;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message);
                    await Warning(ex.Message);
                }
            }

            if (finishedCount == 0)
            {
                Error(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                Successful(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
            }

            Refresh();
        }

        private async Task ExportTemplateImpl(FrameworkElement target)
        {
            if (target is null)
            {
                return;
            }

            var savedlg = new SaveFileDialog
            {
                FileName = SelectedTemplate.Name,
                Filter   = SubSystemString.ModuleFilter
            };

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var fileName = savedlg.FileName;
                var ms       = Xaml.CaptureToBuffer(target);
                var template = TemplateEngine.TemplateDB.FindById(_selectedTemplate.Id);
                var payload  = JSON.Serialize(template);


                await PNG.Write(fileName, payload, ms);
                Successful(SubSystemString.OperationOfSaveIsSuccessful);
            }
            catch (Exception ex)
            {
                Error(SubSystemString.BadModule);

                Xaml.Get<ILogger>()
                    .Warn($"保存模组文件失败,文件名:{savedlg.FileName}，错误原因:{ex.Message}!");
            }
        }

        private async Task ManageManifestImpl()
        {
            await DialogService().Dialog(new ModuleManifestViewModel());
        }

        private async Task<ModuleManifest> PickModuleManifestImpl()
        {
            var r = await SubSystem.OptionSelection<ModuleManifest>(
                SubSystemString.SelectTitle,
                null,
                Property.Manifests);

            return r.IsFinished ? r.Value : null;
        }

        public void Refresh()
        {
            //
            // 加载数据
            Source.Clear();
            Source.AddRange(TemplateEngine.TemplateCacheDB.FindAll());

            MetadataCollection.AddRange(TemplateEngine.MetadataCacheDB.FindAll(), true);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsPresentation"/> 属性。
        /// </summary>
        public bool IsPresentation
        {
            get => _isPresentation;
            set => SetValue(ref _isPresentation, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedTemplate"/> 属性。
        /// </summary>
        public ModuleTemplateCache SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                SetValue(ref _selectedTemplate, value);

                RaiseUpdated(nameof(PresentationIntro));
                RaiseUpdated(nameof(PresentationContractList));
                RaiseUpdated(nameof(PresentationAuthorList));
                RaiseUpdated(nameof(PresentationName));

                if (_selectedTemplate is null)
                {
                    return;
                }

                var template = TemplateEngine.TemplateDB.FindById(_selectedTemplate.Id);
                if (template is null)
                {
                    return;
                }

                ExportTemplateCommand.NotifyCanExecuteChanged();
                PresentationCommand.NotifyCanExecuteChanged();
                RemoveTemplateCommand.NotifyCanExecuteChanged();
                Blocks.AddRange(template.Blocks, true);
                RaiseUpdated(nameof(Presentations));
            }
        }


        public string PresentationIntro => SubSystemString.GetIntro(SelectedTemplate?.Intro);
        public string PresentationContractList => SubSystemString.GetContractList(SelectedTemplate?.ContractList);
        public string PresentationAuthorList => SubSystemString.GetAuthor(SelectedTemplate?.AuthorList);
        public string PresentationName => SubSystemString.GetName(SelectedTemplate?.Name);

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                _sorter.OnNext(_type == DocumentType.None ? Xaml.AlwaysTrue : x => x.ForType == value);
            }
        }


        public ModuleManifestProperty Property { get; }

        public ModuleManifest Ability
        {
            get => Property.GetModuleManifest(DocumentType.Ability);
            set
            {
                Property.SetModuleManifest(DocumentType.Ability, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModuleManifest Character
        {
            get => Property.GetModuleManifest(DocumentType.Character);
            set
            {
                Property.SetModuleManifest(DocumentType.Character, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModuleManifest Geography
        {
            get => Property.GetModuleManifest(DocumentType.Geography);
            set
            {
                Property.SetModuleManifest(DocumentType.Geography, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModuleManifest Item
        {
            get => Property.GetModuleManifest(DocumentType.Item);
            set
            {
                Property.SetModuleManifest(DocumentType.Item, value);
                RaiseUpdated();
                Save();
            }
        }

        public ModuleManifest Other
        {
            get => Property.GetModuleManifest(DocumentType.Other);
            set
            {
                Property.SetModuleManifest(DocumentType.Other, value);
                RaiseUpdated();
                Save();
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleBlock> Blocks { get; }

        public IEnumerable<ModuleBlockDataUI> Presentations => Blocks.Select(ModuleBlockFactory.GetDataUI);

        [NullCheck(UniTestLifetime.Constructor)]
        public SourceList<ModuleTemplateCache> Source { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<MetadataCache> MetadataCollection { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ReadOnlyObservableCollection<ModuleTemplateCache> TemplateCollection => _collection;

        /// <summary>
        /// 
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand PresentationCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ManageManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleTemplateCache> RemoveTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> ExportTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ImportTemplateCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetAbilityManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetCharacterManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetGeographyManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetItemManifestCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand SetOtherManifestCommand { get; }

        public override bool Removable => false;
        public override bool Uniqueness => true;

        public RelayCommand SetAbilityPresentationManifestCommand { get; }
        public RelayCommand SetCharacterPresentationManifestCommand { get; }
        public RelayCommand SetGeographyPresentationManifestCommand { get; }
        public RelayCommand SetItemPresentationManifestCommand { get; }
        public RelayCommand SetOtherPresentationManifestCommand { get; }
    }
}