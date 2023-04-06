using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Models;

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
                   ChartType.Radar =>  new RadarPreviewBlockUI
                    {
                        Source = cpb
                    }
                },
                RarityPreviewBlock rpb2 => new RarityPreviewBlockUI()
                {
                    Source = rpb2
                },
                _ => null
            };
        }

        public abstract void Update(MetadataCollection metadataCollection);

        public string Name { get; protected init; }
        public PreviewBlock BaseSource { get; protected init; }
    }

    public class RarityPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(MetadataCollection metadataCollection)
        {
            // TODO:
        }

        public RarityPreviewBlock Source
        {
            get => (RarityPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Metadata   = value.Metadata;
            }
        }
        public string Metadata { get; init; }
    }

    public class GroupingPreviewBlockUI : PreviewBlockUI
    {       
        public override void Update(MetadataCollection metadataCollection)
        {
            DataLists.ForEach(x => x.Update(metadataCollection));
        }
        
        public GroupingPreviewBlock Source
        {
            get => (GroupingPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
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

    public class HistogramPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(MetadataCollection metadataCollection)
        {
            var unparsedValue = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                                  ?.Value;
            MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                out var axis,
                out var value,
                out var fallback,
                out var max,
                out var min,
                out var color);

            Contract.Assert(value.Length != fallback.Length);

            for (var i = 0; i < value.Length;i++)
            {
                if (value[i] < 0)
                {
                    value[i] = fallback[i];
                }

                value[i] = Math.Clamp(value[i], min, max);
            }

            Color = color;
            Axis.AddRange(axis, true);
            Value.AddRange(value, true);
        }

        public ChartPreviewBlock Source
        {
            get => (ChartPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Metadata   = value.Metadata;
                Color      = "#007ACC";
                Value      = new List<int>();
                Axis       = new List<string>();
            }
        }
        
        public string Metadata { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }

    public class RadarPreviewBlockUI : PreviewBlockUI
    {
        public override void Update(MetadataCollection metadataCollection)
        {
            var unparsedValue = metadataCollection.FirstOrDefault(x => x.Name == Metadata)
                                                  ?.Value;
            MetadataProcessor.ExtractChartBaseFormatted(unparsedValue,
                out var axis,
                out var value,
                out var fallback,
                out var max,
                out var min,
                out var color);

            Contract.Assert(value.Length != fallback.Length);

            for (var i = 0; i < value.Length;i++)
            {
                if (value[i] < 0)
                {
                    value[i] = fallback[i];
                }

                value[i] = Math.Clamp(value[i], min, max);
            }

            Color = color;
            Axis.AddRange(axis, true);
            Value.AddRange(value, true);
        }

        public ChartPreviewBlock Source
        {
            get => (ChartPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Metadata   = value.Metadata;
                Color      = "#007ACC";
                Value      = new List<int>();
                Axis       = new List<string>();
            }
        }
        
        public string Metadata { get; init; }
        public string Color { get; private set; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }
}