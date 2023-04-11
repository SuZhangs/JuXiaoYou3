namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfStickyNote : PartOfDetailPlaceHolder
    {
        public PartOfStickyNote()
        {
            Id = Constants.IdOfStickyNote;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }
    }
}