using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Models.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Models.Previews;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    // TODO:
    // 1) 部分ModuleBlockDataUI需要最小宽度和高度
    // 2) 实现CardAction的替代
    // 3) 实现其他控件的替代
    public class NewPreviewBlockViewModel : ExplicitDialogVM
    {
        private PreviewBlockUI _previewItem;
        private object       _maybeMetadataKind;
        private MetadataKind _type;

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public MetadataKind Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                PreviewItem = PreviewBlockUI.GetUI(ModuleBlockFactory.GetPreviewBlock(_type));
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

                if (value is MetadataKind k)
                {
                    Type = k;
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="PreviewItem"/> 属性。
        /// </summary>
        public PreviewBlockUI PreviewItem
        {
            get => _previewItem;
            set => SetValue(ref _previewItem, value);
        }

        public static Task<Op<PreviewBlock>> New()
        {
            return DialogService()
                .Dialog<PreviewBlock>(new NewPreviewBlockViewModel());
        }

        protected override bool IsCompleted() => _previewItem is not null;

        protected override void Finish()
        {
            Result = ModuleBlockFactory.GetPreviewBlock(_type);
        }

        protected override string Failed() => "未选择";
    }
}