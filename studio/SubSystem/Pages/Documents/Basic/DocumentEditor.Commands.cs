using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        public void Save()
        {
            DocumentEngine.UpdateDocument(_document, _cache);
            SetDirtyState(false);
            Successful(SubSystemString.OperationOfAutoSaveIsSuccessful);
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

        //---------------------------------------------
        //
        // Keywords
        //
        //---------------------------------------------
        private async Task AddKeywordImpl()
        {
            if (Keywords.Count >= 32)
            {
                await Warning(SubSystemString.KeywordTooMany);
            }

            var hash = Keywords.ToHashSet();
            var r    = await StringViewModel.String(SubSystemString.AddKeywordTitle);

            if (!r.IsFinished)
            {
                return;
            }

            if (!hash.Add(r.Value))
            {
                await Warning(Language.ContentDuplicatedText);
                return;
            }

            KeywordEngine.AddKeyword(r.Value);
            Keywords.Add(r.Value);
            SetDirtyState(true);
        }

        private async Task RemoveKeywordImpl(string item)
        {
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (!Keywords.Remove(item))
            {
                return;
            }

            Keywords.Remove(item);
            SetDirtyState(true);
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand ChangeAvatarCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand SaveDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand OpenDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand NewDocumentCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddModulePartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand UpgradeModulePartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PartOfModule> ShiftUpModulePartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PartOfModule> ShiftDownModulePartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PartOfModule> RemoveModulePartCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}