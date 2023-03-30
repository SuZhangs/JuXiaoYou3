namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPlaylist: PartOfDetailPlaceHolder
    {
        public PartOfPlaylist()
        {
            Id = "__Playlist";
        }


        public sealed override bool Removable => true;
    }
}