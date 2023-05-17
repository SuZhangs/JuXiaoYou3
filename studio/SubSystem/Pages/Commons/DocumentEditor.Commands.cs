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
            this.Successful(SubSystemString.OperationOfSaveIsSuccessful);
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            var r = await ImageUtilities.Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (ImageEngine.HasFile(md5))
            {
                var fr = ImageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = ImageUtilities.GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                ImageEngine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ImageEngine.AddFile(record);
            }

            Avatar = avatar;
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