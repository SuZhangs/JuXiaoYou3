using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Inspirations;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private void AddMemberJoinCommandImpl()
        {
            
        }


        private void AddMemberLeaveCommandImpl()
        {
        }


        private void AddMemberMutedCommandImpl()
        {
        }


        private void AddMemberUnMutedCommandImpl()
        {
        }


        private void AddTimestampCommandImpl()
        {
        }


        private  void AddPlainTextCommandImpl()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            if (Speaker is null)
            {
                return;
            }

            var msg = new ChannelMessage
            {
                Content  = Message,
                Id       = ID.Get(),
                MemberID = Speaker.Id,
                Type     = MessageType.PlainText
            };

            //
            //
            AddMessageTo(msg);
            
            //
            //
            Message = string.Empty;
        }


        private void AddEmojiCommandImpl()
        {
        }


        private void AddImageCommandImpl()
        {
        }

        private async Task SetCompositionMessageImpl()
        {
            var r = await MultiLineViewModel.String(Language.GetText("text.AddPendingContent"));

            if (!r.IsFinished)
            {
                return;
            }

            //
            // 
            CompositionMessage = r.Value;
            
            //
            // 
            SetDirtyState();
        }

        private void AddMessageTo(ChannelMessage msg, bool addToChannel = true)
        {
            if (msg is null)
            {
                return;
            }

            var ui = msg.Type switch
            {
                MessageType.PlainText   => new PlainTextUI(msg),
                MessageType.Audio       => expr,
                MessageType.File        => expr,
                MessageType.Image       => expr,
                MessageType.Gif         => expr,
                MessageType.Video       => expr,
                MessageType.RedPacket   => expr,
                MessageType.Recall      => expr,
                MessageType.Call        => expr,
                MessageType.Timestamp   => expr,
                MessageType.Transfer    => expr,
                MessageType.Muted       => expr,
                MessageType.MemberJoin  => expr,
                MessageType.MemberLeave => expr,
                _                       => throw new ArgumentOutOfRangeException()
            };
            
            Messages.Add(ui);
            if(addToChannel) Channel.Messages.Add(msg);
        }
        
        public RelayCommand AddMemberMutedCommand { get; }
        public RelayCommand AddMemberUnMutedCommand { get; }
        public RelayCommand AddTimestampCommand { get; }
        public RelayCommand AddPlainTextCommand { get; }
        public RelayCommand AddEmojiCommand { get; }
        public RelayCommand AddMemberLeaveCommand { get; }
        public RelayCommand AddMemberJoinCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand SwitchSpeakerCommand { get; }
        public RelayCommand RemoveMessageCommand { get; }
        public RelayCommand AddAliasCommand { get; }
        public RelayCommand RemoveAliasCommand { get; }
        public AsyncRelayCommand SetCompositionMessageCommand { get; }
    }
}