namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class Sentence : StorageUIObject
    {
        
    }
    
    public class PartOfSentence : PartOfEditableDetail
    {
        public PartOfSentence()
        {
            Id = Constants.IdOfSentence;
        }
        
        public List<Sentence> Items { get; set; }
    }
}