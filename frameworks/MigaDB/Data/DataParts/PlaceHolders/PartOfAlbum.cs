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
        public List<Album> Items { get; init; }
        
    }
}