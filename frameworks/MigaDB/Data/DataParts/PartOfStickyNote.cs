namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfStickyNote : PartOfDetailPlaceHolder
    {
        public PartOfStickyNote()
        {
            Id = Constants.IdOfStickyNote;
        }

        public List<StickyNote> Items { get; set; }
    }

    public class StickyNote : StorageUIObject
    {
    }
}