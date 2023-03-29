using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using Accessibility;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.Miga.Doc.Parts;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateGallery
{
    public class TemplateGalleryViewModel : TabViewModel
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
        private bool                _isPreview;

        public TemplateGalleryViewModel()
        {
            TemplateEngine = Xaml.Get<IDatabaseManager>()
                                 .GetEngine<TemplateEngine>();

            _sorter            = new BehaviorSubject<Func<ModuleTemplateCache, bool>>(Xaml.AlwaysTrue);
            _reader            = new DataPartReader();
            Source             = new SourceList<ModuleTemplateCache>();
            MetadataCollection = new ObservableCollection<MetadataCache>();
            Blocks             = new ObservableCollection<ModuleBlockDataUI>();

            AddTemplateCommand    = AsyncCommand(AddTemplateImpl);
            ImportTemplateCommand = AsyncCommand(ImportTemplateImpl);
            ExportTemplateCommand = AsyncCommand<FrameworkElement>(ExportTemplateImpl, x => x is not null && SelectedTemplate is not null);
            RemoveTemplateCommand = AsyncCommand<ModuleTemplateCache>(RemoveTemplateImpl);
            PreviewCommand        = Command(() => IsPreview = true);

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

        private async Task AddTemplateImpl()
        {
            var opendlg = new OpenFileDialog
            {
                Filter      = SubSystemString.ModuleFilter,
                Multiselect = true
            };

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
                    await Error(string.Format(Language.GetText(FileNotExists), fileName));
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
                await Error(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                await Successful(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
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
                await Successful(SubSystemString.OperationOfRemoveIsSuccessful);
                Refresh();
            }
        }

        private async Task ImportTemplateImpl()
        {
            var opendlg = new OpenFileDialog
            {
                Filter      = SubSystemString.ModuleFilter,
                Multiselect = true
            };

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
                await Error(string.Format(Language.GetText("text.ImportModulesFailed"), finishedCount, errorCount));
            }
            else
            {
                await Successful(string.Format(Language.GetText("text.ImportModulesSuccessful"), finishedCount, errorCount));
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
                var payload = JSON.Serialize(template);


                await PNG.Write(fileName, payload, ms);
                await Successful(SubSystemString.OperationOfSaveIsSuccessful);
            }
            catch (Exception ex)
            {
                await Error(SubSystemString.BadModule);

                Xaml.Get<ILogger>()
                    .Warn($"保存模组文件失败,文件名:{savedlg.FileName}，错误原因:{ex.Message}!");
            }
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
        /// 获取或设置 <see cref="IsPreview"/> 属性。
        /// </summary>
        public bool IsPreview
        {
            get => _isPreview;
            set => SetValue(ref _isPreview, value);
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
                Blocks.AddRange(template.Blocks.Select(ModuleBlockFactory.GetDataUI), true);
            }
        }


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

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleBlockDataUI> Blocks { get; }

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
        public RelayCommand PreviewCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ModuleTemplateCache> RemoveTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> ExportTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ImportTemplateCommand { get; }
    }
}