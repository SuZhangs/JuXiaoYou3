using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class PlainTextUI : MessageUI
    {

        public PlainTextUI(ChannelMessage msg)
        {
            if (msg is null ||
                msg.Type != MessageType.PlainText)
            {
                throw new ArgumentException(nameof(msg));
            }
            
            MessageSource = msg;
        }
        
        public ChannelMessage MessageSource { get; }

        public override MessageBase Source => MessageSource;
    }
}