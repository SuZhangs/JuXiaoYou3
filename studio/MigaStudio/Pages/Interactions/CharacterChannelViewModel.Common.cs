using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private const char Separator = '\x20';
        
        protected void SwitchSpeakerMemberAfter(string memberID)
        {
            foreach (var message in Messages)
            {
                message.IsSelf = message.MemberID == memberID;
            }
        }

        private void LoadFromChannel()
        {
            if (Channel is null ||
                Cache is null)
            {
                return;
            }
            
            MemberAliasMapper.Clear();
            MemberRoleMapper.Clear();
            MemberTitleMapper.Clear();
            
            //
            // 别名
            foreach (var alias in Channel.Alias)
            {
                var formatted = alias.Split(Separator);

                if (formatted is null ||
                    formatted.Length != 3)
                {
                    continue;
                }

                var source = formatted[0];
                var target = formatted[1];
                var callee = formatted[2];

                if (string.IsNullOrEmpty(source) ||
                    string.IsNullOrEmpty(target) ||
                    string.IsNullOrEmpty(callee))
                {
                    continue;
                }

                if (MemberAliasMapper.TryGetValue(source, out var dict))
                {
                    dict[target] = callee;
                }
                else
                {
                    dict = new Dictionary<string, string>
                    {
                        [target] = callee
                    };
                    MemberAliasMapper.TryAdd(source, dict);
                }
            }

            MemberTitleMapper.AddMany(Channel.Titles);
            MemberRoleMapper.AddMany(Channel.Roles);
        }

        private void SaveToChannel()
        {
            if (Channel is null ||
                Cache is null)
            {
                return;
            }
            
            Channel.Alias.Clear();
            Channel.Roles.Clear();
            Channel.Titles.Clear();
            
            // ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var alias in MemberAliasMapper)
            {
                foreach (var aliasItem in alias.Value)
                {
                    var v = $"{alias.Key}{Separator}{aliasItem.Key}{Separator}{aliasItem.Value}";
                    
                    Channel.Alias.Add(v);
                }
            }
            
            Channel.Titles
                   .AddMany(MemberTitleMapper, true);
            
            Channel.Roles
                   .AddMany(MemberRoleMapper, true);
            
            Channel.Messages
                   .AddMany(Messages.Select(x => x.Source), true);
        }
    }
}