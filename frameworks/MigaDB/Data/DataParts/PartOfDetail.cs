namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfDetail : DataPart, IPartOfDetail
    {
        /// <summary>
        /// 部件类型
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 数据
        /// </summary>
        public List<IPartOfDetailData> Datas { get; init; }
        
        /// <summary>
        /// 索引号 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否为占位符
        /// </summary>
        public bool IsDeclaration => true;
        
        /// <summary>
        /// 是否可移除
        /// </summary>
        public bool Removable { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public abstract class PartOfDetailPlaceHolder : DataPart, IPartOfDetail, IGlobalizationTextSupport
    {
        public string GetLanguageId() => Id;
        
        /// <summary>
        /// 索引号 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否为占位符
        /// </summary>
        public bool IsDeclaration => true;
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }

        /// <summary>
        /// 是否可移除
        /// </summary>
        public abstract bool Removable { get; }
    }
}