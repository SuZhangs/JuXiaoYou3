
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{
    public class NewBlockViewModel : DialogViewModel
    {
        private IModuleBlockDataUI _previewItem;
        private object      _maybeMetadataKind;
        private MetadataKind      _type;

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public MetadataKind Type
        {
            get => _type;
            set
            {
                SetValue(ref _type, value);
                PreviewItem = ModuleBlockFactory.GetDataUI(ModuleBlockFactory.GetBlock(value));
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
        public IModuleBlockDataUI PreviewItem
        {
            get => _previewItem;
            set => SetValue(ref _previewItem, value);
        }
        
        public static Task<Op<ModuleBlock>> New()
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<ModuleBlock>(new NewBlockViewModel());
        }
    }
}