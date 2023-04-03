namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfAlbum : PartOfDetailPlaceHolder
    {
        public PartOfAlbum()
        {
            Id = Constants.IdOfAlbumPart;
        }
        

        public sealed override bool Removable => true;
    }
}