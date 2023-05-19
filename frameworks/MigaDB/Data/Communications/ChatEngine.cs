namespace Acorisoft.FutureGL.MigaDB.Data.Communications
{
    public class ChatEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            throw new NotImplementedException();
        }

        protected override void OnDatabaseClosing()
        {
            throw new NotImplementedException();
        }
        
        public ILiteCollection<ContractList> ContractDB { get; private set; }
        public ILiteCollection<BubbleChannel> ChannelDB { get; private set; }
    }
}