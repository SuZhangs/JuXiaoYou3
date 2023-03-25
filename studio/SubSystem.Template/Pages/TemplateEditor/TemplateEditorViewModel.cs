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

// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    public class TemplateEditorViewModelProxy : BindingProxy<TemplateEditorViewModel>
    {
    }

    public class TemplateEditorViewModel : TabViewModel
    {
        private string       _name;
        private string       _authorList;
        private string       _contractList;
        private int          _version;
        private string       _organizations;
        private string       _intro;
        private DocumentType _forType;
        private string       _for;
        private string       _id;
        private bool         _dirty;

        public TemplateEditorViewModel()
        {
            Id                     = ID.Get();
            Version                = 1;
            Blocks                 = new ObservableCollection<ModuleBlockEditUI>();
            MetadataList           = new ObservableCollection<MetadataCache>();
            OpenPreviewPaneCommand = new RelayCommand(() => IsPreviewPaneOpen = true);

            NewTemplateCommand         = Command(NewTemplateImpl);
            OpenTemplateCommand        = AsyncCommand(OpenTemplateImpl);
            SaveTemplateCommand        = AsyncCommand<FrameworkElement>(SaveTemplateImpl, HasElement);
            NewBlockCommand            = AsyncCommand(NewBlockImpl);
            EditBlockCommand           = AsyncCommand<ModuleBlockEditUI>(EditBlockImpl, HasElement);
            RemoveBlockCommand         = AsyncCommand<ModuleBlockEditUI>(RemoveBlockImpl);
            ShiftUpBlockCommand        = Command<ModuleBlockEditUI>(ShiftUpBlockImpl);
            ShiftDownBlockCommand      = Command<ModuleBlockEditUI>(ShiftDownBlockImpl);
            RemoveAllBlockCommand      = AsyncCommand(RemoveAllBlockImpl);
            RefreshMetadataListCommand = Command(RefreshMetadataListImpl);

            SetDirtyState(false);
        }

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
            catch (Exception)
            {
                await ds.Notify(
                    CriticalLevel.Danger,
                    TemplateSystemString.Notify,
                    TemplateSystemString.BadModule);
            }
        }

        private async Task SaveTemplateImpl(FrameworkElement target)
        {
            // 保存模板的行为是:
            // 1) 判断当前的页面是否保存
            // 2) 选择文件
            // 3) 打开文件并赋值
            var savedlg = new SaveFileDialog
            {
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
                    Version       = _version,
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
                    CriticalLevel.Danger,
                    TemplateSystemString.Notify,
                    TemplateSystemString.OperationOfSaveIsSuccessful);
            }
            catch
            {
                await ds.Notify(
                    CriticalLevel.Success,
                    TemplateSystemString.Notify,
                    TemplateSystemString.BadModule);
            }
        }

        #endregion

        #region Metadata

        private void DetectAll()
        {
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
            MetadataList.Add(new MetadataCache
            {
                Name         = name,
                Id           = ID.Get(),
                MetadataName = meta
            });
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


        /// <summary>
        /// 世界观
        /// </summary>
        public string For
        {
            get => _for;
            set => SetValue(ref _for, value);
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
            set => SetValue(ref _intro, value);
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
            set => SetValue(ref _contractList, value);
        }

        /// <summary>
        /// 作者列表
        /// </summary>
        public string AuthorList
        {
            get => _authorList;
            set => SetValue(ref _authorList, value);
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