﻿using System.Collections.ObjectModel;
using System.Xml.Linq;
using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class ModuleTemplate : ObservableObject
    {
        private string _name;
        private string _authorList;
        private string _contractList;
        private int    _version;
        private string _metadataList;
        private string _organizations;
        private string _copyright;
        private string _intro;
        private int    _forType;
        private string _for;
        
        /// <summary>
        /// 唯一标识符。
        /// </summary>
        [BsonId]
        public string Id { get; init; }
        
        
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
        public int ForType
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
        public string MetadataList
        {
            get => _metadataList;
            set => SetValue(ref _metadataList, value);
        }
        
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
        
        public ObservableCollection<ModuleProperty> Items { get; set; }
    }
}