namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfAlbum : PartOfDetailPlaceHolder
    {
        public PartOfAlbum()
        {
            Id = Constants.IdOfAlbumPart;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }
        
    }
}