namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfSentence : PartOfDetailPlaceHolder
    {
        public PartOfSentence()
        {
            Id = Constants.IdOfSentence;
        }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }
    }
}