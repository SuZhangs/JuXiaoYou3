using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    public class NewTimestampViewModel : ImplicitDialogVM
    {
        protected override bool IsCompleted()
        {
            throw new NotImplementedException();
        }

        protected override void Finish()
        {
            throw new NotImplementedException();
        }

        protected override string Failed()
        {
            throw new NotImplementedException();
        }

        public static Task<Op<ChannelMessage>> New()
        {
            return DialogService()
                       .Dialog<ChannelMessage, NewTimestampViewModel>(new Parameter(args: Array.Empty<object>()));
        }
        
        public static Task<Op<ChannelMessage>> Edit(TimestampUI message)
        {
            return DialogService()
                .Dialog<ChannelMessage, NewTimestampViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        message
                    }
                });
        }
    }
}