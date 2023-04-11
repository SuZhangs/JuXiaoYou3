namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPrototype : PartOfDetailPlaceHolder
    {
        public PartOfPrototype()
        {
            Id = Constants.IdOfPrototypePart;
        }

        public List<Prototype> Items { get; set; }
    }

    public class Prototype : StorageUIObject
    {
    }
}