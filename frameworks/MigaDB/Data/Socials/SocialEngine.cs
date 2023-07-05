namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var db = session.Database;
            CharacterDB = db.GetCollection<MemberCache>(Constants.Name_Chat_Character);
            UpvoteDB    = db.GetCollection<Upvote>(Constants.Name_Chat_Upvote);
            ThreadDB    = db.GetCollection<SocialThread>(Constants.Name_Chat_Thread);
            ChannelDB   = db.GetCollection<Channel>(Constants.Name_Chat_Channel);
        }

        protected override void OnDatabaseClosing()
        {
            CharacterDB = null;
            UpvoteDB    = null;
            ThreadDB    = null;
            ChannelDB   = null;
        }

        public void AddCharacter(MemberCache character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Upsert(character);
        }
        
        public void RemoveCharacter(MemberCache character)
        {
            if (character is null)
            {
                return;
            }

            CharacterDB.Delete(character.Id);
        }

        public void AddChannel(Channel channel)
        {
            if (channel is null)
            {
                return;
            }

            ChannelDB.Upsert(channel);
        }
        
        public void RemoveChannel(Channel channel)
        {
            if (channel is null)
            {
                return;
            }
            ChannelDB.Delete(channel.Id);
        }
        
        public IEnumerable<Channel> GetChannels(string character)
        {
            return ChannelDB.Find(x => x.AvailableMembers
                                        .Any(y => y == character));
        }

        public IEnumerable<MemberCache> GetMembers() => CharacterDB.FindAll();
        
        public ILiteCollection<Channel> ChannelDB { get; private set; }
        public ILiteCollection<SocialThread> ThreadDB { get; private set; }
        public ILiteCollection<MemberCache> CharacterDB { get; private set; }
        public ILiteCollection<Upvote> UpvoteDB { get; private set; }
    }
}