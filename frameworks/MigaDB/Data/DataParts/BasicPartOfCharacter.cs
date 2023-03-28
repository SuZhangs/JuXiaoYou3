namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class BasicPartOfCharacter : FixedDataPart
    {
        protected override FixedDataPart CreateInstance()
        {
            return new BasicPartOfCharacter();
        }
        
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
    }
}