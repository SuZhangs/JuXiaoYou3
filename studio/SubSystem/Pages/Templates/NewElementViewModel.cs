﻿using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class NewElementViewModel : ExplicitDialogVM
    {
        private IModuleBlockDataUI _previewItem;
        private object             _maybeMetadataKind;
        private BlockType          _type;

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public BlockType Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                PresentationItem = ModuleBlockFactory.GetDataUI(ModuleBlockFactory.GetBlock(value));
                CompletedCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="MaybeMetadataKind"/> 属性。
        /// </summary>
        public object MaybeMetadataKind
        {
            get => _maybeMetadataKind;
            set
            {
                SetValue(ref _maybeMetadataKind, value);
                
                if (value is BlockType k)
                {
                    Type = k;
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="PresentationItem"/> 属性。
        /// </summary>
        public IModuleBlockDataUI PresentationItem
        {
            get => _previewItem;
            set => SetValue(ref _previewItem, value);
        }
        
        public static Task<Op<ModuleBlockEditUI>> New()
        {
            return DialogService()
                       .Dialog<ModuleBlockEditUI>(new NewElementViewModel());
        }

        protected override bool IsCompleted() => _previewItem is not null;

        protected override void Finish()
        {
            Result = ModuleBlockFactory.GetEditUI(_type);
        }

        protected override string Failed() => "未选择";
    }
}