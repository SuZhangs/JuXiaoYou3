using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : TabViewModel
    {
        public CharacterChannelViewModel()
        {
            AddPlainTextCommand          = Command(AddPlainTextCommandImpl);
            SetCompositionMessageCommand = AsyncCommand(SetCompositionMessageImpl);
            
        }
        
        private void Save()
        {
            //
            // 
            SetDirtyState(false);
            
            //
            // 保存信息
            SaveToChannel();
        }

        protected override void OnStart(Parameter parameter)
        {
            Channel = new Channel();
            
            //
            // 加载
            LoadFromChannel();

            //
            // 默认发言人
            if (Speaker is null)
            {
            }
            
            base.OnStart(parameter);
        }
    }
}