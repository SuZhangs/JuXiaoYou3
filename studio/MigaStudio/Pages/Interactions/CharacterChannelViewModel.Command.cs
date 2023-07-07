using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using Acorisoft.FutureGL.Forest;
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
        private async Task AddMemberRoleImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            var r = await SubSystem.Selection<MemberRole>("选择成员权限", MemberRole.Manager, new object[]
            {
                MemberRole.Manager,
                MemberRole.Member,
            });
            
            
            if (!r.IsFinished)
            {
                return;
            }

            MemberRoleMapper.Add(cache.Id, r.Value);
            Channel.Roles
                   .Add(cache.Id, r.Value);

            foreach (var support in Messages.OfType<IMessageUpdateSupport>())
            {
                support.Update();
            }
            SetDirtyState();
        }
        
        private async Task RemoveMemberRoleImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (!await this.Error(SR.AreYouSureRemoveIt))
            {
                return;
            }

            MemberRoleMapper.Remove(cache.Id);
            Channel.Roles
                   .Remove(cache.Id);

            foreach (var support in Messages.OfType<IMessageUpdateSupport>())
            {
                support.Update();
            }
            SetDirtyState();
        }


        private async Task AddMemberTitleImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (MemberRoleMapper.TryGetValue(cache.Id, out var role) &&
                (role == MemberRole.Manager || role == MemberRole.Owner))
            {
                this.ErrorNotification("管理员或者群主不能获得头衔");
                return;
            }

            var r = await SingleLineViewModel.String(SR.EditValueTitle);
            
            if (!r.IsFinished)
            {
                return;
            }

            MemberTitleMapper.Add(cache.Id, r.Value);
            Channel.Titles
                   .Add(cache.Id, r.Value);

            foreach (var support in Messages.OfType<IMessageUpdateSupport>())
            {
                support.Update();
            }
            
            SetDirtyState();
        }
        
        private async Task RemoveMemberTitleImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (!await this.Error(SR.AreYouSureRemoveIt))
            {
                return;
            }

            MemberTitleMapper.Remove(cache.Id);
            Channel.Titles
                   .Remove(cache.Id);

            foreach (var support in Messages.OfType<IMessageUpdateSupport>())
            {
                support.Update();
            }
            SetDirtyState();
        }
        
        private async Task RemoveMessageImpl(MessageUI msg)
        {
            if (msg is null)
            {
                return;
            }
            
            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            RemoveMessage(msg);
        }
        
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
                
                // role
                MemberMapper.TryAdd(member.Id, member);
                MemberRoleMapper.TryAdd(member.Id, MemberRole.Member);
                Channel.Roles.Add(member.Id, MemberRole.Member);
                
                // member
                AvailableMemberCollection.Add(member);
                TotalMemberCollection.Add(member);
                Channel.Members.Add(member);
                Channel.AvailableMembers.Add(member.Id);
                Cache.AvailableMembers.Add(member.Id);
                
                AddMessageTo(msg);

                Speaker ??= member;
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
                
                AvailableMemberCollection.Remove(member);
                TotalMemberCollection.Remove(member);
                Channel.AvailableMembers
                       .Remove(member.Id);
                Cache.AvailableMembers
                     .Remove(member.Id);
                AddMessageTo(msg);
            }
        }
        
        private void SwitchSpeakerCommandImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            Speaker = cache;

            if(LatestSpeakers.Count < 10 &&
               !LatestSpeakers.Contains(cache))
            {
                LatestSpeakers.Add(cache);
            }
            else if (!LatestSpeakers.Contains(cache))
            {
                LatestSpeakers.Insert(0, cache);
                LatestSpeakers.RemoveAt(10);
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


        // private void AddEmojiCommandImpl()
        // {
        //     
        // }


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

        public AsyncRelayCommand AddMemberMutedCommand { get; }
        public AsyncRelayCommand AddMemberUnMutedCommand { get; }
        public AsyncRelayCommand AddTimestampCommand { get; }
        public RelayCommand AddPlainTextCommand { get; }
        // public RelayCommand AddEmojiCommand { get; }
        public AsyncRelayCommand AddMemberLeaveCommand { get; }
        public AsyncRelayCommand AddMemberJoinCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand<MemberCache> SwitchSpeakerCommand { get; }
        public AsyncRelayCommand<MessageUI> RemoveMessageCommand { get; }
        public AsyncRelayCommand<MemberCache> AddMemberTitleCommand { get; }
        public AsyncRelayCommand<MemberCache> RemoveMemberTitleCommand { get; }
        public AsyncRelayCommand<MemberCache> AddMemberRoleCommand { get; }
        public AsyncRelayCommand<MemberCache> RemoveMemberRoleCommand { get; }
        public AsyncRelayCommand SaveCommand { get; }
        public AsyncRelayCommand SetCompositionMessageCommand { get; }
    }
}