using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    /// <summary>
    /// 文档缓存
    /// </summary>
    public class DocumentCache : ObservableObject, IDataCache
    {
        private bool   _isLocked;
        private string _avatar;
        private string _name;
        private string _intro;
        
        [BsonId]
        public string Id { get; init; }
        
        public DocumentType Type { get; init; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool Removable { get; init; }
        
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

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
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 关键字
        /// </summary>
        public ObservableCollection<string> Keywords { get; init; }
    }
}