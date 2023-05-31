using System.Collections.Generic;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private void AddLatestSpeaker(CharacterUI character)
        {
            if (LatestSpeakers.Count > 9)
            {
                LatestSpeakers.RemoveAt(LatestSpeakers.Count - 1);
                LatestSpeakers.Insert(0, character);
            }
            else
            {
                LatestSpeakers.Add(character);
            }
            
        }
    }
}