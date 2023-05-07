namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class Appraise : StorageUIObject
    {
        
    }
    
    public class PartOfAppraise : PartOfDetailPlaceHolder
    {
        public PartOfAppraise()
        {
            Id = Constants.IdOfAppraise;
        }
        
        public List<Appraise> Items { get; init; }
    }
}