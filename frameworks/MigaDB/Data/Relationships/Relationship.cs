namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    public enum Relationship
    {
        /// <summary>
        /// 反对关系
        /// </summary>
        Hostile,
        
        /// <summary>
        /// 友好关系
        /// </summary>
        Friendly,
        
        /// <summary>
        /// 中立关系
        /// </summary>
        Neutral,
        
        /// <summary>
        /// 血缘关系
        /// </summary>
        Kinship,
        
        /// <summary>
        /// 无血缘关系，但又法律上的亲属关系（继父继母等）
        /// </summary>
        InLaw
    }
}