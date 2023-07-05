using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Inspirations;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using NLog.Filters;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private async Task AddMemberJoinCommandImpl()
        {
            var members = Studio.Engine<SocialEngine>()
                                .GetMembers()
                                .Where(x => !MemberMapper.ContainsKey(x.Id));
            
            var r = await MemberPickerViewModel.Pick(members);

            if (!r.IsFinished)
            {
                return;
            }

            foreach (var member in r.Value)
            {
                var msg = new ChannelMessage
                {
                    Id       = ID.Get(),
                    MemberID = member.Id,
                    Content  = Language.GetText("text.Interaction.MemberJoin"),
                    Type     = MessageType.MemberJoin
                };
                
                AddMessageTo(msg);
                AvailableMemberCollection.Add(member);
                TotalMemberCollection.Add(member);
                Channel.Members
                       .Add(member);
                Channel.AvailableMembers
                       .Add(member.Id);
            }
        }


        private async Task AddMemberLeaveCommandImpl()
        {
            var r = await MemberPickerViewModel.Pick(AvailableMemberCollection);

            if (!r.IsFinished)
            {
                return;
            }

            foreach (var member in r.Value)
            {
                var msg = new ChannelMessage
                {
                    Id       = ID.Get(),
                    MemberID = member.Id,
                    Content   = Language.GetText("text.Interaction.MemberLeave"),
                    Type = MessageType.MemberLeave
                };
                
                AddMessageTo(msg);
                AvailableMemberCollection.Remove(member);
                Channel.AvailableMembers
                       .Remove(member.Id);
            }
        }


        private async Task AddMemberMutedCommandImpl()
        {
            var r = await MemberPickerViewModel.Pick(AvailableMemberCollection);

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await SingleLineViewModel.String(SR.EditValueTitle);

            if (!r1.IsFinished)
            {
                return;
            }

            r.Value
             .Select(x => new ChannelMessage
             {
                 Id        = ID.Get(),
                 MemberID  = x.Id,
                 Timestamp = r1.Value,
                 Type      = MessageType.Muted
             })
             .ForEach(a => { AddMessageTo(a); });
        }


        private async Task AddMemberUnMutedCommandImpl()
        {
            var r = await MemberPickerViewModel.Pick(Members);

            if (!r.IsFinished)
            {
                return;
            }

            r.Value
             .Select(x => new ChannelMessage
             {
                 Id       = ID.Get(),
                 MemberID = x.Id,
                 Type     = MessageType.UnMuted
             })
             .ForEach(a => { AddMessageTo(a); });
        }


        private async Task AddTimestampCommandImpl()
        {
            if (Speaker is null)
            {
                return;
            }

            var r = await NewTimestampViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            AddMessageTo(r.Value);
        }


        private void AddPlainTextCommandImpl()
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
            var opendlg = Studio.Open(Studio.ImageFilter, true);

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            foreach (var fileName in opendlg.FileNames
                                            .Where(File.Exists))
            {
                var msg = new ChannelMessage
                {
                    Id       = ID.Get(),
                    MemberID = Speaker.Id,
                    Source   = fileName,
                    Type     = MessageType.Image
                };
                
                AddMessageTo(msg);
            }
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

        private void AddMessageTo(MessageBase mb, bool addToChannel = true)
        {
            if (mb is ChannelMessage msg)
            {
                MessageUI ui = msg.Type switch
                {
                    MessageType.PlainText   => new PlainTextUI(msg, FindMemberNameById),
                    MessageType.Emoji       => new EmojiUI(msg, FindMemberNameById),
                    MessageType.Image       => new ImageUI(msg, FindMemberNameById),
                    MessageType.Timestamp   => new TimestampUI(msg),
                    MessageType.Muted       => new MutedAndUnMutedEventUI(msg, FindMemberNameById, FindEventContentByType),
                    MessageType.MemberJoin  => new MemberJoinEventUI(msg, FindMemberNameById),
                    MessageType.MemberLeave => new MemberLeaveEventUI(msg, FindMemberNameById),
                    MessageType.UnMuted     => new MutedAndUnMutedEventUI(msg, FindMemberNameById, FindEventContentByType),
                    _                       => throw new ArgumentOutOfRangeException()
                };

                Messages.Add(ui);
                if (addToChannel) Channel.Messages.Add(msg);
            }
        }

        private string FindMemberNameById(string id)
        {
            if (MemberAliasMapper.TryGetValue(Speaker.Id, out var dict))
            {
                if (dict.TryGetValue(id, out var name))
                {
                    return name;
                }
            }

            return MemberMapper.TryGetValue(id, out var mem) ?
                mem.Name : 
                Language.GetText("text.Untitled");
        }
        
        private string FindEventContentByType(MessageType type,object parameter)
        {
            return type switch
            {
                MessageType.Muted   => string.Format(Language.GetText("text.Interaction.MemberMuted"), parameter),
                MessageType.UnMuted => Language.GetText("text.Interaction.MemberUnMuted"),
                _                   => string.Empty
            };
        }

        public AsyncRelayCommand AddMemberMutedCommand { get; }
        public AsyncRelayCommand AddMemberUnMutedCommand { get; }
        public AsyncRelayCommand AddTimestampCommand { get; }
        public RelayCommand AddPlainTextCommand { get; }
        public RelayCommand AddEmojiCommand { get; }
        public AsyncRelayCommand AddMemberLeaveCommand { get; }
        public AsyncRelayCommand AddMemberJoinCommand { get; }
        public AsyncRelayCommand AddImageCommand { get; }
        public RelayCommand SwitchSpeakerCommand { get; }
        public RelayCommand RemoveMessageCommand { get; }
        public RelayCommand AddAliasCommand { get; }
        public RelayCommand RemoveAliasCommand { get; }
        public AsyncRelayCommand SetCompositionMessageCommand { get; }
    }
}