﻿
using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public class ComposeCache : ObservableObject, IDataCache
    {
        [BsonId]
        public string Id { get; init; }

        public DocumentType Type { get; init; }
        
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool Removable { get; init; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        
        private bool _isLocked;

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked
        {
            get => _isLocked;
            set => SetValue(ref _isLocked, value);
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TimeOfCreated { get; init; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime TimeOfModified { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 关键字
        /// </summary>
        public ObservableCollection<string> Keywords { get; init; }
    }
}