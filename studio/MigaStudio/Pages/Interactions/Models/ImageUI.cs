using System.IO;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class ImageUI : MessageUI
    {
        public ImageUI(ChannelMessage msg)
        {
            if (msg is null ||
                msg.Type != MessageType.Image)
            {
                throw new ArgumentException(nameof(msg));
            }

            MessageSource = msg;
        }


        public ChannelMessage MessageSource { get; }
        public string ImageSource => MessageSource.Source;

        public override MessageBase Source => MessageSource;
        public bool IsExists => File.Exists(MessageSource.Source);
    }
}