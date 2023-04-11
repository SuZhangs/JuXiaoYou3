namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class Album : StorageObject
    {
            
        public string Source { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }
    
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