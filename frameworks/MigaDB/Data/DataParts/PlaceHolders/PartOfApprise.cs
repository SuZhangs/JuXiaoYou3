namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class Apprise : StorageUIObject
    {
        
    }
    
    public class PartOfApprise : PartOfDetailPlaceHolder
    {
        public PartOfApprise()
        {
            Id = Constants.IdOfApprise;
        }
        
        public List<Apprise> Items { get; init; }
    }
}