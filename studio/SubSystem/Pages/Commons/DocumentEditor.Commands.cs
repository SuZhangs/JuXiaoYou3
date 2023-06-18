using System.IO;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        public void Save()
        {
            SavePresentationPart();
            DocumentEngine.UpdateDocument(Document, Cache);
            SetDirtyState(false);
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            await ImageUtilities.Avatar(ImageEngine, x => Avatar = x);
        }

        //
        //
        private async Task NewDocumentImpl()
        {
           var cache = await DocumentUtilities.AddDocument(DocumentEngine);

           if (!cache.IsFinished)
           {
               return;
           }
           
           //
           //
           DocumentUtilities.OpenDocument(Controller, cache.Value);
        }

        //---------------------------------------------
        //
        // Keywords
        //
        //---------------------------------------------


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ChangeAvatarCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }
    }
}