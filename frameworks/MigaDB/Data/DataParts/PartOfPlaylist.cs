namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPlaylist : PartOfDetailPlaceHolder
    {
        public PartOfPlaylist()
        {
            Id = Constants.IdOfPlaylistPart;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> DataBags { get; init; }
    }
}