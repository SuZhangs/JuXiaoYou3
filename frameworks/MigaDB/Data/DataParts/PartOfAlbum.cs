namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfAlbum : PartOfDetailPlaceHolder
    {
        public PartOfAlbum()
        {
            Id = "__Album";
        }


        public sealed override bool Removable => true;
    }
}