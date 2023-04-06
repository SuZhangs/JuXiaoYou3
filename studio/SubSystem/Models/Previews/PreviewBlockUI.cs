using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaUtils.Foundation;

namespace Acorisoft.FutureGL.MigaStudio.Models.Previews
{
    public abstract class PreviewBlockUI : StorageUIObject
    {
        public static PreviewBlockUI GetUI(PreviewBlock block)
        {
            return block switch
            {
                GroupingPreviewBlock gpb => new GroupingPreviewBlockUI
                {
                    Source = gpb
                },
                ChartPreviewBlock cpb => cpb.ChartType switch
                {
                    ChartType.Histogram => new HistogramPreviewBlockUI
                    {
                        Source = cpb
                    },
                    ChartType.Radar => new RadarPreviewBlockUI
                    {
                        Source = cpb
                    }
                },
                RarityPreviewBlock rpb2 => new RarityPreviewBlockUI()
                {
                    Source = rpb2
                },
                StringPreviewBlock sp => new StringPreviewBlockUI
                {
                    Source = sp
                },
                _                     => null
            };
        }

        public abstract void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker);

        private string _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        public PreviewBlock BaseSource { get; protected init; }
    }

    public class RarityPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            // TODO:
        }

        public RarityPreviewBlock Source
        {
            get => (RarityPreviewBlock)BaseSource;
            init
            {
                Name = value.Name;
                BaseSource = value;
                Metadata   = value.ValueSourceID;
            }
        }

        public string Metadata { get; init; }
    }

    public class GroupingPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            DataLists.ForEach(x => x.Update(metadataTracker, blockTracker));
        }

        public GroupingPreviewBlock Source
        {
            get => (GroupingPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Name = value.Name;
                NameLists  = new ObservableCollection<string>();
                DataLists  = new ObservableCollection<PreviewBlockDataUI>();

                foreach (var data in value.DataLists)
                {
                    NameLists.Add(data.Name);
                    DataLists.Add(PreviewBlockDataUI.GetDataUI(data));
                }
            }
        }

        public ObservableCollection<string> NameLists { get; private init; }
        public ObservableCollection<PreviewBlockDataUI> DataLists { get; private init; }
    }
    
    public class StringPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            Value = metadataTracker(ValueSource)?.Value
                                                .SubString(200);
        }

        public StringPreviewBlock Source
        {
            get => (StringPreviewBlock)BaseSource;
            init
            {
                BaseSource  = value;
                Name        = value.Name;
                ValueSource = value.ValueSourceID;
            }
        }

        private string _value;

        public string ValueSource { get; init; }
        
        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
    }

    public class HistogramPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            var block = (ChartBlock)blockTracker(ValueSource);

            if (BaseSource.IsMetadata)
            {
                var unparsedValue = metadataTracker(ValueSource)?.Value;
                MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                    out var axis,
                    out var value,
                    out _,
                    out _,
                    out _,
                    out var color);
                Color = color;
                Axis.AddRange(axis, true);
                Value.AddRange(value, true);
            }
            else
            {
                Color = block.Color;
                Axis.AddRange(block.Axis, true);
                Value.AddRange(block.Value, true);
            }
        }

        public ChartPreviewBlock Source
        {
            get => (ChartPreviewBlock)BaseSource;
            init
            {
                BaseSource  = value;
                Name = value.Name;
                ValueSource = value.ValueSourceID;
                Color       = "#007ACC";
                Value       = new List<int>();
                Axis        = new List<string>();
            }
        }

        public string ValueSource { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }

    public class RadarPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(Func<string, Metadata> metadataTracker, Func<string, ModuleBlock> blockTracker)
        {
            var block = (ChartBlock)blockTracker(ValueSource);

            if (BaseSource.IsMetadata)
            {
                var unparsedValue = metadataTracker(ValueSource)?.Value;
                MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                    out var axis,
                    out var value,
                    out _,
                    out _,
                    out _,
                    out var color);
                Color = color;
                Axis.AddRange(axis, true);
                Value.AddRange(value, true);
            }
            else
            {
                Color = block.Color;
                Axis.AddRange(block.Axis, true);
                Value.AddRange(block.Value, true);
            }
        }

        public ChartPreviewBlock Source
        {
            get => (ChartPreviewBlock)BaseSource;
            init
            {
                BaseSource  = value;
                Name = value.Name;
                ValueSource = value.ValueSourceID;
                Color       = "#007ACC";
                Value       = new List<int>();
                Axis        = new List<string>();
            }
        }

        public string ValueSource { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }
}