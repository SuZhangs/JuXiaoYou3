using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateGallery
{
    public class TemplateGalleryViewModel : TabViewModel
    {
        [NullCheck(UniTestLifetime.Constructor)]
        private readonly ReadOnlyObservableCollection<ModuleTemplateCache> _collection;
        
        [NullCheck(UniTestLifetime.Constructor)]
        private readonly BehaviorSubject<Func<ModuleTemplateCache, bool>>  _sorter;


        private DocumentType _type;
        
        public TemplateGalleryViewModel()
        {
            TemplateEngine = Xaml.Get<IDatabaseManager>()
                                 .GetEngine<TemplateEngine>();
            
            _sorter            = new BehaviorSubject<Func<ModuleTemplateCache, bool>>(Xaml.AlwaysTrue);
            Source             = new SourceList<ModuleTemplateCache>();
            MetadataCollection = new ObservableCollection<MetadataCache>();

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
                Filter = TemplateSystemString.ModuleFilter
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            var fileName = opendlg.FileName;
            
            if (!File.Exists(fileName))
            {
            }
        }
        
        private async Task RemoveTemplateImpl(ModuleTemplateCache item)
        {
            
        }
        
        private async Task ImportTemplateImpl()
        {
            var opendlg = new OpenFileDialog
            {
                Filter = TemplateSystemString.ModuleFilter
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
        
        private async Task ExportTemplateImpl()
        {
            
        }

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set => SetValue(ref _type, value);
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