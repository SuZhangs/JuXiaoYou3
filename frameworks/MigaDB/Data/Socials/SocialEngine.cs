namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            
        }

        protected override void OnDatabaseClosing()
        {
        }
        
        public ILiteCollection<SocialChannel> ChannelDB { get; private set; }
        public ILiteCollection<SocialThread> ThreadDB { get; private set; }
        public ILiteCollection<Upvote> UpvoteDB { get; private set; }
    }
}