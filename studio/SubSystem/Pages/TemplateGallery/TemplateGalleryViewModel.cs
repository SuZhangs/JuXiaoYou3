using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
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

        [NullCheck(UniTestLifetime.Constructor)] private readonly ReadOnlyObservableCollection<ModuleTemplateCache> _collection;
        [NullCheck(UniTestLifetime.Constructor)] private readonly BehaviorSubject<Func<ModuleTemplateCache, bool>>  _sorter;
        [NullCheck(UniTestLifetime.Constructor)] private readonly DataPartReader                                    _reader;
        
        private DocumentType _type;

        public TemplateGalleryViewModel()
        {
            TemplateEngine = Xaml.Get<IDatabaseManager>()
                                 .GetEngine<TemplateEngine>();

            _sorter            = new BehaviorSubject<Func<ModuleTemplateCache, bool>>(Xaml.AlwaysTrue);
            _reader            = new DataPartReader();
            Source             = new SourceList<ModuleTemplateCache>();
            MetadataCollection = new ObservableCollection<MetadataCache>();

            AddTemplateCommand    = AsyncCommand(AddTemplateImpl);
            ImportTemplateCommand = AsyncCommand(ImportTemplateImpl);
            ExportTemplateCommand = AsyncCommand(ExportTemplateImpl);
            RemoveTemplateCommand = AsyncCommand<ModuleTemplateCache>(RemoveTemplateImpl);
            
            Source.Connect()
                  .Filter(_sorter)
                  .ObserveOn(Scheduler)
                  .Bind(out _collection)
                  .Subscribe()
                  .DisposeWith(Collector);
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
            var fileNames = opendlg.FileNames;
            var logger = Xaml.Get<ILogger>();


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
                    var msg = Language.GetEnum(r.Reason);
                        
                    logger.Warn(msg);
                    await Warning(msg);
                }
                else
                {
                    await Successful(SubSystemString.OperationOfAddIsSuccessful);
                }
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
                Filter = SubSystemString.ModuleFilter
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var logger = Xaml.Get<ILogger>();
            try
            {
                var result      = await _reader.ReadAsync(opendlg.FileName);
                var oldTemplate = result.Result;
                var template    = ModuleBlockFactory.Upgrade(oldTemplate);
                var r           = TemplateEngine.AddModule(template);

                if (!r.IsFinished)
                {
                    var msg = Language.GetEnum(r.Reason);
                        
                    logger.Warn(msg);
                    await Warning(msg);
                }
                else
                {
                    await Successful(SubSystemString.OperationOfAddIsSuccessful);
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
                await Warning(ex.Message);
            }
        }

        private async Task ExportTemplateImpl()
        {
            await Task.Delay(300);
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
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                _sorter.OnNext(x => x.ForType == value);
            }
        }

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
        public AsyncRelayCommand<ModuleTemplateCache> RemoveTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ExportTemplateCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ImportTemplateCommand { get; }
    }
}