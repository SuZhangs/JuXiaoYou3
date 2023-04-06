using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Models.Previews;
using ListBox = Acorisoft.FutureGL.Forest.Controls.ListBox;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs
{
    public class EditPreviewBlockViewModel : ExplicitDialogVM
    {
        public static Task<Op<object>> Edit(GroupingPreviewBlock hb, DataPartCollection dataPartCollection)
        {
            var blockCollection = dataPartCollection.Where(x => x is PartOfModule)
                                                       .Cast<PartOfModule>()
                                                       .Select(x => x.Blocks)
                                                       .SelectMany(x => x)
                                                       .Where(x => !string.IsNullOrEmpty(x.Metadata) && x is not GroupBlock)
                                                       .Where(x => x.ExtractMetadata()
                                                                    .Type == hb.Type);
            return Xaml.Get<IDialogService>()
                       .Dialog(new EditPreviewBlockViewModel(), new RouteEventArgs
                       {
                           Args = new object[]
                           {
                               hb,
                               blockCollection
                           }
                       });
        }
        
        protected override bool IsCompleted() => true;

        protected override void OnStart(RouteEventArgs parameter)
        {
            Block = parameter.Args[0] as GroupingPreviewBlock;
            var array = parameter.Args[1] as IEnumerable<ModuleBlock>;
            Templates.AddRange(array, true);
        }

        protected override void Finish()
        {
            if (TargetElement is null)
            {
                return;
            }

            var r = TargetElement.SelectedItems
                                 .Cast<ModuleBlock>()
                                 .ToArray();
            Block.DataLists
                 .AddRange(Create(r));
        }

        private IEnumerable<IPreviewBlockData> Create(IEnumerable<ModuleBlock> blockCollection)
        {
            return Block.Type switch
            {
                MetadataKind.Text     => CreateText(blockCollection),
                MetadataKind.Degree   => CreateDegree(blockCollection),
                MetadataKind.Star     => CreateStar(blockCollection),
                MetadataKind.Heart    => CreateHeart(blockCollection),
                MetadataKind.Progress => CreateProgress(blockCollection),
                MetadataKind.Switch   => CreateSwitch(blockCollection),
                MetadataKind.Color    => CreateColor(blockCollection),
                _                     => throw new InvalidOperationException(),
            };
        }

        private static IEnumerable<IPreviewBlockData> CreateText(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewTextData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        
        private static IEnumerable<IPreviewBlockData> CreateDegree(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewDegreeData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        
        private static IEnumerable<IPreviewBlockData> CreateStar(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewStarData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        
        private static IEnumerable<IPreviewBlockData> CreateHeart(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewHeartData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        
        private static IEnumerable<IPreviewBlockData> CreateProgress(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewProgressData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateSwitch(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewSwitchData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        private static IEnumerable<IPreviewBlockData> CreateColor(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x => new PreviewColorData
            {
                Name     = x.Name,
                Metadata = x.Metadata
            });
        }
        protected override string Failed()
        {
            return string.Empty;
        }

        private ListBox _targetElement;

        /// <summary>
        /// 获取或设置 <see cref="TargetElement"/> 属性。
        /// </summary>
        public ListBox TargetElement
        {
            get => _targetElement;
            set => SetValue(ref _targetElement, value);
        }

        public GroupingPreviewBlock Block { get; private set; }
        public ObservableCollection<ModuleBlock> Templates { get; } = new ObservableCollection<ModuleBlock>();
    }
}