using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class InteractionStartupViewModel : InteractionViewModelBase
    {
        public InteractionStartupViewModel()
        {
            SocialEngine   = Studio.Engine<SocialEngine>();
            DocumentEngine = Studio.Engine<DocumentEngine>();
            
            Characters     = new ObservableCollection<MemberCache>();

            AddCharacterCommand = AsyncCommand(AddCharacterImpl);
        }

        protected override void OnStart()
        {
            Characters.AddMany(SocialEngine.GetMembers(), true);
            base.OnStart();
        }

        protected override void OnResume()
        {
            if (Version != SocialEngine.Version)
            {
                Version = SocialEngine.Version;
                Characters.AddMany(SocialEngine.GetMembers(), true);
            }
            
            base.OnResume();
        }

        private async Task AddCharacterImpl()
        {
            var hash = Characters.Select(x => x.Id)
                                 .ToHashSet();

            var r = await NewMemberViewModel.Add(hash);

            if (!r.IsFinished)
            {
                return;
            }

            foreach (var member in r.Value)
            {
                SocialEngine.AddCharacter(member);
                Characters.Add(member);
            }
        }

        public AsyncRelayCommand AddCharacterCommand { get; }
        public AsyncRelayCommand<MemberCache> RemoveCharacterCommand { get; }
        public SocialEngine SocialEngine { get; }
        public DocumentEngine DocumentEngine { get; }
        public ObservableCollection<MemberCache> Characters { get; }
    }
}