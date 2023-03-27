using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;
using NLog;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    public class TemplateEditorViewModelProxy : BindingProxy<TemplateEditorViewModel>
    {
    }

    public class TemplateEditorViewModel : TabViewModel
    {

        private readonly HashSet<string> _metaHashSet;
            
        private string          _name;
        private string          _authorList;
        private string          _contractList;
        private int             _version;
        private string          _organizations;
        private string          _intro;
        private DocumentType    _forType;
        private string          _for;
        private string          _id;
        private bool            _dirty;


        public TemplateEditorViewModel()
        {
            _metaHashSet             =  new HashSet<string>();
            Id                       =  ID.Get();
            Version                  =  1;
            ForType                  =  DocumentType.CharacterDocument;
            Blocks                   =  new ObservableCollection<ModuleBlockEditUI>();
            Blocks.CollectionChanged += (_, _) => RaiseUpdated(nameof(PreviewBlocks));
            MetadataList             =  new ObservableCollection<MetadataCache>();
            OpenPreviewPaneCommand   =  new RelayCommand(() => IsPreviewPaneOpen = true);

            NewTemplateCommand         = Command(NewTemplateImpl);
            OpenTemplateCommand        = AsyncCommand(OpenTemplateImpl);
            SaveTemplateCommand        = AsyncCommand<FrameworkElement>(SaveTemplateImpl);
            NewBlockCommand            = AsyncCommand(NewBlockImpl);
            EditBlockCommand           = AsyncCommand<ModuleBlockEditUI>(EditBlockImpl, HasElement);
            RemoveBlockCommand         = AsyncCommand<ModuleBlockEditUI>(RemoveBlockImpl, HasElement);
            ShiftUpBlockCommand        = Command<ModuleBlockEditUI>(ShiftUpBlockImpl, HasElement);
            ShiftDownBlockCommand      = Command<ModuleBlockEditUI>(ShiftDownBlockImpl, HasElement);
            RemoveAllBlockCommand      = AsyncCommand(RemoveAllBlockImpl);
            RefreshMetadataListCommand = Command(RefreshMetadataListImpl);

            SetDirtyState(false);
        }

        #region Translate

        // TODO: 翻译
        private static string GetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "世界观：未知"
                };
            }

            return value;
        }
        
        private static string GetFor(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "世界观名字"
                };
            }
            
            var pattern = Language.Culture switch
            {
                _ => "世界观:{0}",
            };

            return string.Format(pattern, value);
        }
        
        private static string GetAuthor(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "作者：佚名"
                };
            }
            
            var pattern = Language.Culture switch
            {
                _ => "作者：{0}",
            };

            return string.Format(pattern, value);
        }
        
        
        private static string GetContractList(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "联系方式：暂无"
                };
            }
            
            var pattern = Language.Culture switch
            {
                _ => "联系方式：{0}",
            };

            return string.Format(pattern, value);
        }
        
        
        private static string GetIntro(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "简介：暂无"
                };
            }

            return value;
        }
        

        #endregion

        private static bool HasElement<T>(T element) where T : class => element is not null;

        private void SetDirtyState(bool value)
        {
            _dirty           = value;
            ApprovalRequired = value;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            var name = string.IsNullOrEmpty(Name) ? Id : Name;
            SetTitle(name, _dirty);
        }

        #region Templates

        private void NewTemplateImpl()
        {
            // 新建模板的行为是:
            // 1) 直接弹出新的标签页
            Controller.New<TemplateEditorViewModel>();
        }

        // private ModuleTemplate OpenOrUpgrade(string payload)
        // {
        //     try
        //     {
        //
        //     }
        //     catch()
        //     {
        //         
        //     }
        // }


        private async Task OpenTemplateImpl()
        {
            // 打开模板的行为是:
            // 1) 判断当前的页面是否保存
            // 2) 选择文件
            // 3) 打开文件并赋值
            var ds = Xaml.Get<IDialogService>();

            // 行为 1)
            if (_dirty)
            {
                var r = await ds.Warning(TemplateSystemString.Notify, TemplateSystemString.AreYouSureCreateNew);

                if (!r) return;
            }

            var opendlg = new OpenFileDialog
            {
                Filter = TemplateSystemString.ModuleFilter
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                // 3) 打开文件并赋值
                var fileName        = opendlg.FileName;
                var templatePayload = await PNG.ReadDataAsync(fileName);
                var template        = JSON.FromJson<ModuleTemplate>(templatePayload);

                Id            = template.Id;
                Version       = template.Version;
                Name          = template.Name;
                Intro         = template.Intro;
                Organizations = template.Organizations;
                ContractList  = template.ContractList;
                For           = template.For;
                ForType       = template.ForType;
                
                SetDirtyState(false);
                Blocks.AddRange(template.Blocks.Select(ModuleBlockFactory.GetEditUI), true);
                MetadataList.AddRange(template.MetadataList, true);
            }
            catch (Exception ex)
            {
                await ds.Notify(
                    CriticalLevel.Danger,
                    TemplateSystemString.Notify,
                    TemplateSystemString.BadModule);
                
                Xaml.Get<ILogger>()
                    .Warn($"打开模组文件失败,文件名:{opendlg.FileName}，错误原因:{ex.Message}!");
                          
            }
        }

        private async Task SaveTemplateImpl(FrameworkElement target)
        {
            if(target is null)
            {
                return;
            }

            // 保存模板的行为是:
            // 1) 判断当前的页面是否保存
            // 2) 选择文件
            // 3) 打开文件并赋值
            
            var savedlg = new SaveFileDialog
            {
                FileName = PreviewName,
                Filter = TemplateSystemString.ModuleFilter
            };

            var ds = Xaml.Get<IDialogService>();

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var fileName = savedlg.FileName;
                var ms       = Xaml.CaptureToBuffer(target);
                var template = new ModuleTemplate
                {
                    Id            = _id,
                    Name          = _name,
                    Intro         = _intro,
                    AuthorList    = _authorList,
                    ContractList  = _contractList,
                    Organizations = _organizations,
                    Version       = ++Version,
                    MetadataList  = MetadataList.ToList(),
                    Blocks = Blocks.Select(x => x.CreateInstance())
                                   .ToList(),
                    ForType = _forType,
                    For     = _for
                };
                var payload = JSON.Serialize(template);


                await PNG.Write(fileName, payload, ms);
                SetDirtyState(false);
                await ds.Notify(
                    CriticalLevel.Success,
                    TemplateSystemString.Notify,
                    TemplateSystemString.OperationOfSaveIsSuccessful);
            }
            catch(Exception ex)
            {
                await ds.Notify(
                    CriticalLevel.Danger,
                    TemplateSystemString.Notify,
                    TemplateSystemString.BadModule);
                
                Xaml.Get<ILogger>()
                    .Warn($"保存模组文件失败,文件名:{savedlg.FileName}，错误原因:{ex.Message}!");
            }
        }

        #endregion

        #region Metadata

        private void DetectAll()
        {
            MetadataList.Clear();
            _metaHashSet.Clear();
            foreach (var block in Blocks)
            {
                DetectMetadata(block);
            }
        }
        
        private void DetectMetadata(IModuleBlockEditUI element)
        {
            if (element is GroupBlockEditUI gbe)
            {
                foreach (var subItem in gbe.Items)
                {
                    if (subItem is null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(subItem.Metadata))
                    {
                        continue;
                    }

                    AddOrUpdateMetadata(subItem.Name, subItem.Metadata);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(element.Metadata))
                {
                    return;
                }

                AddOrUpdateMetadata(element.Name, element.Metadata);
            }
        }

        private void AddOrUpdateMetadata(string name, string meta)
        {
            if (_metaHashSet.Add(meta))
            {
                MetadataList.Add(new MetadataCache
                {
                    Name         = name,
                    Id           = ID.Get(),
                    MetadataName = meta
                });
            }
        }

        #endregion

        #region Block

        private async Task NewBlockImpl()
        {
            var r = await NewBlockViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            var element = r.Value;
            Blocks.Add(element);


            await EditBlockViewModel.Edit(element);
            DetectAll();
            SetDirtyState(true);
        }

        private async Task EditBlockImpl(ModuleBlockEditUI element)
        {
            var r = await EditBlockViewModel.Edit(element);

            if (!r.IsFinished)
            {
                return;
            }

            DetectAll();
            SetDirtyState(true);
            RaiseUpdated(nameof(PreviewBlocks));

            await Xaml.Get<IDialogService>()
                      .Notify(
                          CriticalLevel.Success,
                          TemplateSystemString.Notify,
                          TemplateSystemString.OperationOfSaveIsSuccessful);
        }

        private async Task RemoveBlockImpl(ModuleBlockEditUI element)
        {
            var r = await Xaml.Get<IDialogService>()
                              .Danger(
                                  TemplateSystemString.Notify,
                                  Language.RemoveAllItemText);

            if (!r)
            {
                return;
            }

            Blocks.Remove(element);
            DetectAll();
            SetDirtyState(true);
        }

        private void ShiftUpBlockImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var targetIndex = Blocks.IndexOf(element);

            if (targetIndex <= 0)
            {
                return;
            }

            Blocks.Move(targetIndex, targetIndex - 1);
        }

        private void ShiftDownBlockImpl(ModuleBlockEditUI element)
        {
            if (element is null)
            {
                return;
            }

            var targetIndex = Blocks.IndexOf(element);

            if (targetIndex < 0 ||
                targetIndex >= Blocks.Count - 1)
            {
                return;
            }

            Blocks.Move(targetIndex, targetIndex + 1);
        }

        private async Task RemoveAllBlockImpl()
        {
            var r = await Xaml.Get<IDialogService>()
                              .Danger(
                                  TemplateSystemString.Notify,
                                  Language.RemoveAllItemText);

            if (!r)
            {
                return;
            }

            //
            // 清空
            Blocks.Clear();
            MetadataList.Clear();
            SetDirtyState(true);
        }

        #endregion

        private void RefreshMetadataListImpl()
        {
            DetectAll();
        }

        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }


        public string PreviewFor => GetFor(_for);
        public string PreviewIntro => GetFor(_intro);
        public string PreviewContractList => GetContractList(_contractList);
        public string PreviewAuthorList => GetAuthor(_authorList);
        public string PreviewName => GetName(_name);

        /// <summary>
        /// 世界观
        /// </summary>
        public string For
        {
            get => _for;
            set
            {
                SetValue(ref _for, value);
                RaiseUpdated(nameof(PreviewFor));
            }
        }

        /// <summary>
        /// 模组类型
        /// </summary>
        public DocumentType ForType
        {
            get => _forType;
            set => SetValue(ref _forType, value);
        }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro
        {
            get => _intro;
            set
            {
                SetValue(ref _intro, value);
                RaiseUpdated(nameof(PreviewIntro));
            }
        }

        /// <summary>
        /// 组织
        /// </summary>
        public string Organizations
        {
            get => _organizations;
            set => SetValue(ref _organizations, value);
        }

        /// <summary>
        /// 元数据列表
        /// </summary>
        public ObservableCollection<MetadataCache> MetadataList { get; init; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Version
        {
            get => _version;
            set => SetValue(ref _version, value);
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContractList
        {
            get => _contractList;
            set
            {
                SetValue(ref _contractList, value);
                RaiseUpdated(nameof(PreviewContractList));
            }
        }

        /// <summary>
        /// 作者列表
        /// </summary>
        public string AuthorList
        {
            get => _authorList;
            set
            {
                SetValue(ref _authorList, value);
                RaiseUpdated(nameof(PreviewAuthorList));
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                SetValue(ref _name, value);
                UpdateTitle();
                RaiseUpdated(nameof(PreviewName));
            }
        }

        private ModuleBlockEditUI _selectedBlock;

        /// <summary>
        /// 获取或设置 <see cref="SelectedBlock"/> 属性。
        /// </summary>
        public ModuleBlockEditUI SelectedBlock
        {
            get => _selectedBlock;
            set
            {
                SetValue(ref _selectedBlock, value);
                EditBlockCommand.NotifyCanExecuteChanged();
                RemoveBlockCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isPreviewPaneOpen;

        /// <summary>
        /// 获取或设置 <see cref="IsPreviewPaneOpen"/> 属性。
        /// </summary>
        public bool IsPreviewPaneOpen
        {
            get => _isPreviewPaneOpen;
            set => SetValue(ref _isPreviewPaneOpen, value);
        }

        /// <summary>
        /// 模组内容块集合。
        /// </summary>
        public ObservableCollection<ModuleBlockEditUI> Blocks { get; init; }

        public IEnumerable<ModuleBlockDataUI> PreviewBlocks =>
            Blocks.Select(x => ModuleBlockFactory.GetDataUI(x.CreateInstance()));

        public RelayCommand NewTemplateCommand { get; }
        public AsyncRelayCommand OpenTemplateCommand { get; }
        public AsyncRelayCommand<FrameworkElement> SaveTemplateCommand { get; }
        public AsyncRelayCommand NewBlockCommand { get; }
        public AsyncRelayCommand<ModuleBlockEditUI> EditBlockCommand { get; }
        public AsyncRelayCommand<ModuleBlockEditUI> RemoveBlockCommand { get; }
        public RelayCommand<ModuleBlockEditUI> ShiftUpBlockCommand { get; }
        public RelayCommand<ModuleBlockEditUI> ShiftDownBlockCommand { get; }
        public AsyncRelayCommand RemoveAllBlockCommand { get; }
        public RelayCommand OpenPreviewPaneCommand { get; }
        public RelayCommand RefreshMetadataListCommand { get; }
    }
}