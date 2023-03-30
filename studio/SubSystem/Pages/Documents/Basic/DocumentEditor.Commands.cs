using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        [Obsolete]
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

        [Obsolete]
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
                Warning("没有变化");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            
        }
        
        //---------------------------------------------
        //
        // Commands
        //
        //---------------------------------------------
        
        
        
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
    }
}