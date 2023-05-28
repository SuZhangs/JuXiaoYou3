﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using Acorisoft.Miga.Doc.Groups;

namespace Acorisoft.Miga.Doc.Documents
{
    [DebuggerDisplay("{DocumentType}-{Name}")]
    public class DocumentIndex : SnapshotObject
    {
        private bool   _isAssociated;
        private string _avatar;
        private bool   _isLocking;
        private string _summary;

        public DocumentIndexCopy CreateCopy()
        {
            return new DocumentIndexCopy
            {
                Id      = Id,
                Name    = Name,
                Avatar  = Avatar,
                Summary = Summary
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string OwnerId { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => SetValue(ref _summary, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public EntityType EntityType { get; set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="IsLocking"/> 属性。
        /// </summary>
        public bool IsLocking
        {
            get => _isLocking;
            set => SetValue(ref _isLocking, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDateTime { get; set; }
        
        /// <summary>
        /// 获取或设置当前文档索引的头像。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="IsAssociated"/> 属性。
        /// </summary>
        public bool IsAssociated
        {
            get => _isAssociated;
            set => SetValue(ref _isAssociated, value);
        }
        
        /// <summary>
        /// 标签
        /// </summary>
        public ObservableCollection<string> Keywords { get; init; }
        
        /// <summary>
        /// 获取或设置当前文档索引的类型
        /// </summary>
        public DocumentKind DocumentType { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public SkillType SkillType { get; set; }
        public string Rarity { get; set; }
        public int Slot { get; set; }
        
        public GroupType GroupType { get; init; }
    }

    public class DocumentIndexCopy : ObservableObject
    {
        public string Id { get; init; }
        public string Avatar { get; init; }
        public string Name { get; init; }
        public string Summary { get; init; }
    } 
}