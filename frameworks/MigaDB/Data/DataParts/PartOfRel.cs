namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfRel : PartOfDetailPlaceHolder
    {
        public PartOfRel()
        {
            Id = "__Relationship_Character";
        }


        public sealed override bool Removable => false;
    }
}