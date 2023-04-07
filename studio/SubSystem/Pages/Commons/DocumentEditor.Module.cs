using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.Miga.Doc.Parts;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private readonly Dictionary<string, DataPart>             _DataPartTrackerOfId;
        private readonly Dictionary<Type, DataPart>               _DataPartTrackerOfType;
        private readonly Dictionary<string, ModuleBlock>          _BlockTrackerOfId;

        private ModuleBlock GetBlockById(string id)
        {
            return _BlockTrackerOfId.TryGetValue(id, out var b) ? b : null;
        }
        
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
                .Dialog<IEnumerable<ModuleTemplateCache>>(new ModuleSelectorViewModel(), new RoutingEventArgs
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
                    Document.Parts.Add(module);
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
                AddBlock(module.Name, module.Blocks);

                return true;
            }

            return false;
        }

        private void AddBlock(string name, IList<ModuleBlock> blocks)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                var block    = blocks[i];
                var metadata = block.Metadata;

                if (block is GroupBlock gb)
                {
                    AddBlock(gb.Name, gb.Items);
                }
                else
                {
                    if (!_BlockTrackerOfId.TryAdd(block.Id, block))
                    {
                        SensitiveOperation($"模组:{name}, 内容块：{block.Name}的ID与现存的内容块冲突，请升级")
                            .GetAwaiter()
                            .GetResult();
                        continue;
                    }
                
                    if (string.IsNullOrEmpty(metadata))
                    {
                        continue;
                    }

                    if (_MetadataTrackerByName.ContainsKey(metadata))
                    {
                        blocks.RemoveAt(i);
                    }
                    else
                    {
                        AddMetadata(block.ExtractMetadata());
                    }
                }
            }
        }

        private async Task RemoveModulePartImpl(PartOfModule module)
        {
            if (module is null)
            {
                return;
            }

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
            foreach (var block in module.Blocks)
            {
                if (block is GroupBlock g)
                {
                    foreach (var b in g.Items)
                    {
                        _BlockTrackerOfId.Remove(b.Id);
                        if(!string.IsNullOrEmpty(block.Metadata))
                            RemoveMetadata(b.ExtractMetadata());
                    }
                }
                else
                {
                    _BlockTrackerOfId.Remove(block.Id);
                    if(!string.IsNullOrEmpty(block.Metadata))
                        RemoveMetadata(block.ExtractMetadata());
                }
            }

            RemoveModulePart(module);
            ResortModuleParts();
        }

        private void RemoveModulePart(PartOfModule module)
        {
            Document.Parts.Remove(module);
            ModuleParts.Remove(module);
            _DataPartTrackerOfId.Remove(module.Id);
            _DataPartTrackerOfType.Remove(module.GetType());
        }

        private bool CanUpgrade(PartOfModule module, out ModuleTemplate template)
        {
            var id = module.Id;
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
                    var index = Document.Parts.IndexOf(module);
                    Document.Parts[index] = newModule;

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
                SetDirtyState();
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
            SetDirtyState();
        }


        private void ShiftUpModulePartImpl(PartOfModule module)
        {
            ModuleParts.ShiftUp(module, (_, _, _) => ResortModuleParts());
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedModulePart"/> 属性。
        /// </summary>
        public PartOfModule SelectedModulePart
        {
            get => _selectedModulePart;
            set
            {
                SetValue(ref _selectedModulePart, value);

                if (_selectedModulePart is null)
                {
                    ContentBlocks.Clear();
                }
                else
                {
                    var selector = _selectedModulePart.Blocks
                                                      .Select(x => ModuleBlockFactory.GetDataUI(x, OnModuleBlockValueChanged));
                    ContentBlocks.AddRange(selector, true);
                    RemoveModulePartCommand.NotifyCanExecuteChanged();
                    ShiftDownModulePartCommand.NotifyCanExecuteChanged();
                    ShiftUpModulePartCommand.NotifyCanExecuteChanged();
                }
            }
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<PartOfModule> ModuleParts { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<ModuleBlockDataUI> ContentBlocks { get; }

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
    }
}