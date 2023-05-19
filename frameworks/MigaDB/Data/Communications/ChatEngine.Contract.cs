using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.Communications
{
    /// <summary>
    /// <see cref="Contract"/> 表示一个联系方式
    /// </summary>
    public class Contract : ObservableObject
    {
        /// <summary>
        /// 原始引用
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Source { get; init; }

        /// <summary>
        /// 备注
        /// </summary>
        private string _alias;

        /// <summary>
        /// 获取或设置 <see cref="Alias"/> 属性。
        /// </summary>
        public string Alias
        {
            get => _alias;
            set => SetValue(ref _alias, value);
        }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; init; }
    }

    public class ContractList : ObservableObject
    {
        private string                         _name;
        private ObservableCollection<Contract> _list;
        private string                         _owner;

        /// <summary>
        /// 获取或设置 <see cref="Owner"/> 属性。
        /// </summary>
        public string Owner
        {
            get => _owner;
            set => SetValue(ref _owner, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public ObservableCollection<Contract> List
        {
            get => _list;
            set => SetValue(ref _list, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}