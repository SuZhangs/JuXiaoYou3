using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.Miga.Doc.Parts;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        
        private async Task AddModulePartImpl()
        {
            //
            // 只能添加未添加的模组
            var availableModules = TemplateEngine.TemplateCacheDB
                                                 .FindAll()
                                                 .Where(x => !_DataPartTrackerOfId.ContainsKey(x.Id) && x.ForType == Type);
            
            //
            // 返回用户选择的模组
            var moduleCaches = await DialogService()
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
            
            
            ResortModuleParts();
        }

        private bool CanUpgrade(PartOfModule module,out ModuleTemplate template)
        {
            var id                      = module.Id;
            var maybeNewVersionTemplate = TemplateEngine.TemplateDB
                                                        .FindById(id);

            if (maybeNewVersionTemplate is null ||
                module.Version >= maybeNewVersionTemplate.Version)
            {
                template = null;
                return false;
            }

            template = maybeNewVersionTemplate;
            return true;
        }

        private void UpgradeModule(
            PartOfModule oldModule,
            PartOfModule newModule, 
            IList<ModuleBlock> added, 
            IList<ModuleBlock> removed,
            IList<ModuleBlock> modified)
        {
            
            //
            // 以oldModule作为基准
            var hashSet = oldModule.Blocks
                                   .ToDictionary(x => x.Id, x => x);
            
            //
            //

            foreach (var newBlock in newModule.Blocks)
            {
                if (hashSet.ContainsKey(newBlock.Id))
                {
                    modified.Add(newBlock);
                }
                else
                {
                    added.Add(newBlock);
                }

                hashSet.Remove(newModule.Id);
            }

            
            //
            // 删除Block
            foreach (var removedBlock in oldModule.Blocks.Where(x => hashSet.ContainsKey(x.Id)))
            {
                if (string.IsNullOrEmpty(removedBlock.Metadata))
                {
                    continue;
                }
                
                RemoveMetadata(removedBlock.Metadata);
            }

            foreach (var addedBlock in added)
            {
                if (string.IsNullOrEmpty(addedBlock.Metadata))
                {
                    continue;
                }
                
                AddMetadata(addedBlock.ExtractMetadata());
            }

            foreach (var newBlock in modified)
            {
                var oldBlock = hashSet[newBlock.Id];

                if (oldBlock.GetType() == newBlock.GetType())
                {
                    //
                    // 内容复制
                    oldBlock.CopyTo(newBlock);
                }
                
                //
                //
                AddMetadata(newBlock.ExtractMetadata());
            }
            
            added.Clear();
            removed.Clear();
            modified.Clear();
        }

        private void UpgradeModulePartImpl()
        {
            var count              = 0;
            var upgradeCount       = 0;
            var modifiedCollection = new List<ModuleBlock>(32);
            var addedCollection    = new List<ModuleBlock>(32);
            var removedCollection  = new List<ModuleBlock>(32);
            
            foreach (var module in ModuleParts)
            {
                if (CanUpgrade(module, out var template))
                {
                    var newModule = MigaDB.Data
                                          .Templates
                                          .TemplateEngine
                                          .CreateModule(template);
                    UpgradeModule(
                        module,
                        newModule, 
                        addedCollection,
                        removedCollection,
                        modifiedCollection);

                    //
                    // 替换文档中的部件
                    var index = _document.Parts.IndexOf(module);
                    _document.Parts[index] = newModule;

                    //
                    // 替换当前的部件
                    index              = ModuleParts.IndexOf(module);
                    ModuleParts[index] = newModule;

                    //
                    // 替换当前显示的部件
                    if (ReferenceEquals(_selectedModulePart, module))
                    {
                        SelectedModulePart = newModule;
                    }
                    
                    upgradeCount++;
                }

                count++;
            }

            if (upgradeCount == 0)
            {
                Info("没有需要升级的模板");
            }
            else
            {
                SetDirtyState(true);
                Successful($"添加成功，完成升级{upgradeCount}个，总计:{count}");
            }
        }
        
        private void ShiftDownModulePartImpl(PartOfModule module)
        {
            ModuleParts.ShiftDown(module, (_, _, _) => ResortModuleParts());
        }

        private void ResortModuleParts()
        {
            for (var i = 0; i < ModuleParts.Count; i++)
            {
                ModuleParts[i].Index = i;
            }
            ShiftDownModulePartCommand.NotifyCanExecuteChanged();
            ShiftUpModulePartCommand.NotifyCanExecuteChanged();
            SetDirtyState(true);
        }
        
        
        private void ShiftUpModulePartImpl(PartOfModule module)
        {
            ModuleParts.ShiftUp(module, (_, _, _) => ResortModuleParts());
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

            ResortModuleParts();
            if (result == 0)
            {
                Warning(SubSystemString.NoChange);
            }
            else
            {
                Successful(SubSystemString.OperationOfAddIsSuccessful);
            }
        }
        
        private bool AddModule(PartOfModule module)
        {
            if (module is null)
            {
                return false;
            }
            
            if (_DataPartTrackerOfId.TryAdd(module.Id, module))
            {
                module.Index = ModuleParts.Count;
                ModuleParts.Add(module);

                for (var i = 0; i < module.Blocks.Count; i++)
                {
                    var block    = module.Blocks[i];
                    var metadata = block.Metadata;
                    
                    if (string.IsNullOrEmpty(metadata))
                    {
                        continue;
                    }

                    if (_MetadataTrackerByName.ContainsKey(metadata))
                    {
                        module.Blocks.RemoveAt(i);
                    }
                    else
                    {
                        AddMetadata(block.ExtractMetadata());
                    }
                }
                return true;
            }

            return false;
        }
    }
}