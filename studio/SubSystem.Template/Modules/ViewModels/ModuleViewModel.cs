using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Utils;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Modules.ViewModels
{
    public class ModuleViewModel : ViewModelBase
    {
        private readonly Dictionary<string, int> _metadataMap;

        private string       _name;
        private string       _authorList;
        private string       _contractList;
        private int          _version;
        private string       _organizations;
        private string       _copyright;
        private string       _intro;
        private DocumentType _forType;
        private string       _for;
        private string       _id;

        public ModuleViewModel()
        {
            _metadataMap = new Dictionary<string, int>();
            MetadataList  = new ObservableCollection<MetadataCache>();
            Blocks        = new ObservableCollection<ModuleBlock>();
        }

        public void NewTemplate()
        {
            _name          = string.Empty;
            _authorList    = string.Empty;
            _contractList  = string.Empty;
            _organizations = string.Empty;
            _copyright     = string.Empty;
            _intro         = string.Empty;
            _for           = string.Empty;
            _id            = ID.Get();
            _version       = 1;
            _forType       = DocumentType.CharacterConstraint;
            _metadataMap.Clear();
            MetadataList.Clear();
            Blocks.Clear();
        }

        private void OpenTemplate(ModuleTemplate template)
        {
            _name          = template.Name;
            _authorList    = template.AuthorList;
            _contractList  = template.ContractList;
            _organizations = template.Organizations;
            _copyright     = template.Copyright;
            _intro         = template.Intro;
            _for           = template.For;
            _id            = template.Id;
            _version       = template.Version;
            _forType       = template.ForType;
            _metadataMap.Clear();
            
            foreach (var cache in template.MetadataList)
            {
                _metadataMap.Add(cache.Id, cache.RefCount);
                MetadataList.Add(cache);
            }
            
            MetadataList.Clear();
            Blocks.Clear();
            Blocks.AddRange(template.Blocks);
        }

        public void AddBlock(ModuleBlock block)
        {
            if (block is null)
            {
                return;
            }

            if (Blocks.Contains(block))
            {
                return;
            }
            
            Blocks.Add(block);
        }
        
        
        public void EditBlock(ModuleBlock block)
        {
            if (block is null)
            {
                return;
            }
            
            if (!Blocks.Contains(block))
            {
                return;
            }
        }

        public void RemoveBlock(ModuleBlock block)
        {
            if (block is null)
            {
                return;
            }
            
            Blocks.Remove(block);
        }

        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="For"/> 属性。
        /// </summary>
        public string For
        {
            get => _for;
            set => SetValue(ref _for, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ForType"/> 属性。
        /// </summary>
        public DocumentType ForType
        {
            get => _forType;
            set => SetValue(ref _forType, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Copyright"/> 属性。
        /// </summary>
        public string Copyright
        {
            get => _copyright;
            set => SetValue(ref _copyright, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Organizations"/> 属性。
        /// </summary>
        public string Organizations
        {
            get => _organizations;
            set => SetValue(ref _organizations, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="MetadataList"/> 属性。
        /// </summary>
        public ObservableCollection<MetadataCache> MetadataList { get; init; }
        
        /// <summary>
        /// 获取或设置 <see cref="Version"/> 属性。
        /// </summary>
        public int Version
        {
            get => _version;
            set => SetValue(ref _version, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ContractList"/> 属性。
        /// </summary>
        public string ContractList
        {
            get => _contractList;
            set => SetValue(ref _contractList, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="AuthorList"/> 属性。
        /// </summary>
        public string AuthorList
        {
            get => _authorList;
            set => SetValue(ref _authorList, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 模组内容块集合。
        /// </summary>
        public ObservableCollection<ModuleBlock> Blocks { get; init; }

        public RelayCommand AddBlockCommand { get; }
        public RelayCommand EditBlockCommand { get; }
        public RelayCommand RemoveBlockCommand { get; }
        public RelayCommand AddItemCommand { get; }
        public RelayCommand EditItemCommand { get; }
        public RelayCommand RemoveItemCommand { get; }
        public RelayCommand SaveTemplateCommand { get; }
        public RelayCommand OpenTemplateCommand { get; }
        public RelayCommand NewTemplateCommand { get; }
    }
}