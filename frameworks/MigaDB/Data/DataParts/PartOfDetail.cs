namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfDetail : DataPart, IPartOfDetail
    {
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
    }
    
    public class PartOfDetailPlaceHolder : DataPart, IPartOfDetail
    {
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
        public bool Removable { get; init; }
    }
}