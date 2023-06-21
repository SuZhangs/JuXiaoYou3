using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Inspirations;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
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


        private void AddPlainTextCommandImpl()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return;
            }

            // var msg = new SocialMessage
            // {
            //     Content = Message,
            //     Id = ID.Get(),
            //     MemberID = Speaker.Id,
            // };

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

        private async Task SetPendingContentImpl()
        {
            var r = await MultiLineViewModel.String(Language.GetText("text.AddPendingContent"));

            if (!r.IsFinished)
            {
                return;
            }

            //
            // 
            PendingContent = r.Value;
            
            //
            // 
            SetDirtyState();
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
        public AsyncRelayCommand SetPendingContentCommand { get; }
    }
}