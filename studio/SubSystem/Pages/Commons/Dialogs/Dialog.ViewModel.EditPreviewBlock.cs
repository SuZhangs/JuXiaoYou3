﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaStudio.Models.Previews;
using Acorisoft.Miga.Doc.Parts;
using TagLib.Riff;
using ListBox = Acorisoft.FutureGL.Forest.Controls.ListBox;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class EditPreviewBlockViewModel : ExplicitDialogVM
    {
        
        public static Task<Op<object>> Edit(GroupingPreviewBlock hb, DataPartCollection dataPartCollection)
        {
            var blockCollection = new List<ModuleBlock>(64);
            var blocks = dataPartCollection.Where(x => x is PartOfModule)
                                           .OfType<PartOfModule>()
                                           .Select(x => x.Blocks)
                                           .SelectMany(x => x)
                                           .ToArray();

            var simpleBlock = blocks.Where(x => x is not GroupBlock)
                                    .Where(x => x.ExtractType == hb.Type);

            var blockInGroup = blocks.Where(x => x is GroupBlock)
                                     .OfType<GroupBlock>()
                                     .SelectMany(x => x.Items)
                                     .Where(x => x.ExtractType == hb.Type);

            blockCollection.AddRange(blockInGroup);
            blockCollection.AddRange(simpleBlock);
            return Xaml.Get<IDialogService>()
                       .Dialog(new EditPreviewBlockViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               hb,
                               blockCollection
                           }
                       });
        }
        
        

        protected override bool IsCompleted() => true;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var param = parameter.Parameter;
            Block = param.Args[0] as GroupingPreviewBlock;
            var array = param.Args[1] as IEnumerable<ModuleBlock>;
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
                MetadataKind.Text       => CreateText(blockCollection),
                MetadataKind.Degree     => CreateDegree(blockCollection),
                MetadataKind.Star       => CreateStar(blockCollection),
                MetadataKind.Heart      => CreateHeart(blockCollection),
                MetadataKind.Progress   => CreateProgress(blockCollection),
                MetadataKind.Switch     => CreateSwitch(blockCollection),
                MetadataKind.Color      => CreateColor(blockCollection),
                MetadataKind.Likability => CreateLikability(blockCollection),
                MetadataKind.Rate       => CreateRate(blockCollection),
                _                       => throw new InvalidOperationException(),
            };
        }

        private static IEnumerable<IPreviewBlockData> CreateText(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewTextData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateDegree(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewDegreeData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateStar(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewStarData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateHeart(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewHeartData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateProgress(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1;
                if (x is INumberBlock number)
                {
                    scale = 100 / number.Maximum;
                }

                return new PreviewProgressData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateLikability(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1;

                if (x is INumberBlock number)
                {
                    scale = 100 / number.Maximum;
                }

                return new PreviewLikabilityData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }
        
        
        private static IEnumerable<IPreviewBlockData> CreateRate(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                var scale = 1;

                if (x is INumberBlock number)
                {
                    scale = 100 / number.Maximum;
                }

                return new PreviewRateData
                {
                    Name          = x.Name,
                    Scale         = scale,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }

        private static IEnumerable<IPreviewBlockData> CreateSwitch(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewSwitchData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
            });
        }


        private static IEnumerable<IPreviewBlockData> CreateColor(IEnumerable<ModuleBlock> blockCollection)
        {
            return blockCollection.Select(x =>
            {
                var useID = string.IsNullOrEmpty(x.Metadata);
                return new PreviewColorData
                {
                    Name          = x.Name,
                    ValueSourceID = useID ? x.Id : x.Metadata,
                    IsMetadata    = !useID
                };
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