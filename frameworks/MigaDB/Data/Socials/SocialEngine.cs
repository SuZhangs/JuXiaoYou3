namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var db = session.Database;
            CharacterDB = db.GetCollection<SocialCharacter>(Constants.Name_Chat_Character);
            UpvoteDB    = db.GetCollection<Upvote>(Constants.Name_Chat_Upvote);
            ThreadDB    = db.GetCollection<SocialThread>(Constants.Name_Chat_Thread);
            ChannelDB   = db.GetCollection<SocialChannel>(Constants.Name_Chat_Channel);
        }

        protected override void OnDatabaseClosing()
        {
            CharacterDB = null;
            UpvoteDB    = null;
            ThreadDB    = null;
            ChannelDB   = null;
        }

        public void AddCharacter(SocialCharacter character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Upsert(character);
        }
        
        public void RemoveCharacter(SocialCharacter character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Delete(character.Id);
        }

        public void AddChannel(SocialChannel channel)
        {
            if (channel is null)
            {
                return;
            }

            ChannelDB.Upsert(channel);
        }
        
        public void RemoveChannel(SocialChannel channel)
        {
            if (channel is null)
            {
                return;
            }

            ChannelDB.Delete(channel.Id);
        }
        
        public IEnumerable<SocialChannel> GetChannels(string character)
        {
            return ChannelDB.FindAll()
                            .Where(x => x.Members
                                         .Any(y => y.MemberID == character));
        }
        
        public ILiteCollection<SocialChannel> ChannelDB { get; private set; }
        public ILiteCollection<SocialThread> ThreadDB { get; private set; }
        public ILiteCollection<SocialCharacter> CharacterDB { get; private set; }
        public ILiteCollection<Upvote> UpvoteDB { get; private set; }
    }
}