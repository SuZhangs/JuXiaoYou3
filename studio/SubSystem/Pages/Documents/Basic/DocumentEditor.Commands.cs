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
        private void SaveDocumentImpl()
        {
            DocumentEngine.UpdateDocument(_document, _cache);
            SetDirtyState(false);
            Successful(SubSystemString.OperationOfAutoSaveIsSuccessful);
        }
        
        private async Task AddModulePartImpl()
        {
            //
            // 只能添加未添加的模组
            var availableModules = TemplateEngine.TemplateCacheDB
                                                 .FindAll()
                                                 .Where(x => !_DataPartTrackerOfId.ContainsKey(x.Id));
            
            //
            // 返回用户选择的模组
            var moduleCaches = await Xaml.Get<IDialogService>()
                                         .Dialog<IEnumerable<ModuleTemplateCache>>(new ModuleSelectorViewModel(), new RouteEventArgs
                                         {
                                             Args = new object[]
                                             {
                                                 availableModules
                                             }
                                         });

            if (!moduleCaches.IsFinished)
            {
                return;
            }

            var module = moduleCaches.Value
                                     .Select(x => TemplateEngine.CreateModule(x));

            AddModules(module);
        }

        private async Task RemoveModulePartImpl(PartOfModule module)
        {
            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            //
            // 删除当前内容
            if (ReferenceEquals(SelectedModulePart, module))
            {
                SelectedModulePart = null;
            }
            
            //
            // 删除Metadata
            foreach (var block in module.Blocks
                                        .Where(x => !string.IsNullOrEmpty(x.Metadata)))
            {
                RemoveMetadata(block.ExtractMetadata());
            }
            
            
        }


        private void AddModules(IEnumerable<PartOfModule> modules)
        {
            if (modules is null)
            {
                return;
            }

            var result = 0;
            
            foreach (var module in modules)
            {
                if (AddModule(module))
                {
                    _document.Parts.Add(module);
                    result++;
                }
            }

            if (result == 0)
            {
                Warning(SubSystemString.NoChange);
            }
            else
            {
                Successful(SubSystemString.OperationOfAddIsSuccessful);
            }
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
                avatar = $"avatar_{ID.Get()}.png";
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
        
        
        [NullCheck(UniTestLifetime.Constructor)]public AsyncRelayCommand ChangeAvatarCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand SaveDocumentCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand OpenDocumentCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand NewDocumentCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<IPartOfDetail> ShiftUpDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<IPartOfDetail> ShiftDownDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<IPartOfDetail> RemoveDetailPartCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand UpgradeModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> ShiftUpModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> ShiftDownModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> RemoveModulePartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfModule> RemoveAllModulePartCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddKeywordCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<string> RemoveKeywordCommand { get; }
    }
}