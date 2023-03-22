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
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Win32;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
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
            Id      = ID.Get();
            Version = 1;
            Blocks = new ObservableCollection<ModuleBlock>();

            NewTemplateCommand         = Command(NewTemplateImpl);
            OpenTemplateCommand        = AsyncCommand(OpenTemplateImpl);
            SaveTemplateCommand        = AsyncCommand<FrameworkElement>(SaveTemplateImpl, HasElement);
            NewBlockCommand            = AsyncCommand(NewBlockImpl);
            EditBlockCommand           = AsyncCommand<ModuleBlock>(EditBlockImpl, HasElement);
            RemoveBlockCommand         = AsyncCommand(RemoveBlockImpl);
            ShiftUpBlockCommand        = AsyncCommand(ShiftUpBlockImpl);
            ShiftDownBlockCommand      = AsyncCommand(ShiftDownBlockImpl);
            RemoveAllBlockCommand      = AsyncCommand(RemoveAllBlockImpl);
            NewElementCommand          = AsyncCommand(NewElementImpl);
            EditElementCommand         = AsyncCommand(EditElementImpl);
            ShiftUpElementCommand      = AsyncCommand(ShiftUpElementImpl);
            ShiftDownElementCommand    = AsyncCommand(ShiftDownElementImpl);
            RemoveElementCommand       = AsyncCommand(RemoveElementImpl);
            RefreshMetadataListCommand = AsyncCommand(RefreshMetadataListImpl);
            
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

        private void NewTemplateImpl()
        {
            Controller.New<TemplateEditorViewModel>();
        }

        private async Task OpenTemplateImpl()
        {
            var ds = Xaml.Get<IDialogService>();

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

                Blocks.AddRange(template.Blocks, true);
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
                    Blocks        = Blocks.ToList(),
                    ForType       = _forType,
                    For           = _for
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

        private async Task NewBlockImpl()
        {
            var r = await NewBlockViewModel.New();

            if (!r.IsFinished)
            {
                return;
            }

            Blocks.Add(r.Value);
        }

        private async Task EditBlockImpl(ModuleBlock element)
        {
            var r = await EditBlockViewModel.New(element);

            if (!r.IsFinished)
            {
                return;
            }
            
            
        }

        private async Task RemoveBlockImpl()
        {
        }

        private async Task ShiftUpBlockImpl()
        {
        }

        private async Task ShiftDownBlockImpl()
        {
        }

        private async Task RemoveAllBlockImpl()
        {
        }

        private async Task NewElementImpl()
        {
        }

        private async Task EditElementImpl()
        {
        }

        private async Task ShiftUpElementImpl()
        {
        }

        private async Task ShiftDownElementImpl()
        {
        }

        private async Task RemoveElementImpl()
        {
        }

        private async Task RefreshMetadataListImpl()
        {
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

        /// <summary>
        /// 模组内容块集合。
        /// </summary>
        public ObservableCollection<ModuleBlock> Blocks { get; init; }

        public RelayCommand NewTemplateCommand { get; }
        public AsyncRelayCommand OpenTemplateCommand { get; }
        public AsyncRelayCommand<FrameworkElement> SaveTemplateCommand { get; }
        public AsyncRelayCommand NewBlockCommand { get; }
        public AsyncRelayCommand<ModuleBlock> EditBlockCommand { get; }
        public AsyncRelayCommand RemoveBlockCommand { get; }
        public AsyncRelayCommand ShiftUpBlockCommand { get; }
        public AsyncRelayCommand ShiftDownBlockCommand { get; }
        public AsyncRelayCommand RemoveAllBlockCommand { get; }
        public AsyncRelayCommand NewElementCommand { get; }
        public AsyncRelayCommand EditElementCommand { get; }
        public AsyncRelayCommand ShiftUpElementCommand { get; }
        public AsyncRelayCommand ShiftDownElementCommand { get; }
        public AsyncRelayCommand RemoveElementCommand { get; }
        public AsyncRelayCommand RefreshMetadataListCommand { get; }
    }
}