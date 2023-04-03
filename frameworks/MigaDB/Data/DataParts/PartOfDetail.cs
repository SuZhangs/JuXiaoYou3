﻿namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public abstract class PartOfDetail : DataPart, IPartOfDetail
    {
        
        /// <summary>
        /// 索引号 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否为占位符
        /// </summary>
        public abstract bool IsDeclaration { get; }
        
        /// <summary>
        /// 是否可移除
        /// </summary>
        public abstract bool Removable { get; }
    }

    public sealed class PartOfPlugin : PartOfDetail
    {
        /// <summary>
        /// 部件类型
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 是否为占位符
        /// </summary>
        public override bool IsDeclaration => false;

        /// <summary>
        /// 是否可移除
        /// </summary>
        public override bool Removable => true;
        
        /// <summary>
        /// 数据
        /// </summary>
        public List<IPartOfDetailData> Datas { get; init; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public abstract class PartOfDetailPlaceHolder : PartOfDetail, IPartOfDetail, IGlobalizationTextSupport
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLanguageId() => Id;

        /// <summary>
        /// 是否为占位符
        /// </summary>
        public sealed override bool IsDeclaration => true;

        /// <summary>
        /// 是否可移除
        /// </summary>
        public sealed override bool Removable => false;
    }
}