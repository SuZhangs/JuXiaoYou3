using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class Directory : StorageObject
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Owner"/> 属性。
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }
    }

    public class DirectoryUI : ObservableObject
    {
        public string Id => Source.Id;
        
        /// <summary>
        /// 
        /// </summary>
        public Directory Source { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Index    
        {
            get => Source.Index;
            set => Source.Index = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Owner
        {
            get => Source.Owner;
            set => Source.Owner = value;
        }
        
        /// <summary>
        /// 子级
        /// </summary>
        public ObservableCollection<DirectoryUI> Children { get; init; }

        /// <summary>
        /// 获取或设置名字
        /// </summary>
        public string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }
    }
}