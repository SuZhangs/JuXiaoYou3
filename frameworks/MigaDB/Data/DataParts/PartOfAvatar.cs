namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfAvatar : FixedDataPart
    {
        protected sealed override FixedDataPart CreateInstance()
        {
            return new PartOfAvatar();
        }

        public string Avatar { get; set; }
    }
}