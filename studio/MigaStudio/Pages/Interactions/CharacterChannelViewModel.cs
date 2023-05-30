using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : TabViewModel
    {
        public CharacterChannelViewModel()
        {
            Messages = new ObservableCollection<ChatMessageUI>
            {
                new PlainTextMessageUI
                {
                    
                    Avatar = "D:\\2.png",
                    Name = "Sender",
                    Role = MemberRole.Owner,
                    Content = typeof(CharacterChannelViewModel).FullName,
                    IsSelf = false,
                    Style = PreferSocialMessageLayout.QQ,
                },
                new PlainTextMessageUI
                {
                    
                    Avatar  = "D:\\2.png",
                    Name    = "Character 1",
                    Role    = MemberRole.Self,
                    Content = "rashiua",
                    IsSelf  = true,
                    Style   = PreferSocialMessageLayout.QQ,
                },   new PlainTextMessageUI
                {
                    
                    Avatar  = "D:\\2.png",
                    Name    = "Character 2",
                    Role    = MemberRole.Manager,
                    Content = "false",
                    IsSelf  = false,
                    Style   = PreferSocialMessageLayout.QQ,
                },
            };
        }
        
        public ObservableCollection<ChatMessageUI> Messages { get; }
    }
}