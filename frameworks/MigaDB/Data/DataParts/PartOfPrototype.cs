namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPrototype : PartOfDetailPlaceHolder
    {
        public PartOfPrototype()
        {
            Id = Constants.IdOfPrototypePart;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }
    }
}