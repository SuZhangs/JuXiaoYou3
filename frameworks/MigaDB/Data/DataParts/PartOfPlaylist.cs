namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPlaylist : PartOfDetailPlaceHolder
    {
        public PartOfPlaylist()
        {
            Id = Constants.IdOfPlaylistPart;
        }


        public sealed override bool Removable => true;
    }
}