using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class Character : StorageUIObject
    {
        private string _intro;
        
        /// <summary>
        /// 原始的
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Source { get; init; }
        
        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<Contract> ContractList { get; init; }


        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
    }
}