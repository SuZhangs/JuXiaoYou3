using System.Collections.ObjectModel;
using System.Windows.Forms;
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
                HistogramPreviewBlock hpb => new HistogramPreviewBlockUI
                {
                    Source = hpb
                },
                RadarPreviewBlock rpb => new RadarPreviewBlockUI
                {
                    Source = rpb
                },
                RarityPreviewBlock rpb2 => new RarityPreviewBlockUI()
                {
                    Source = rpb2
                },
                _ => null
            };
        }

        public string Name { get; protected init; }
        public PreviewBlock BaseSource { get; protected init; }
    }

    public class RarityPreviewBlockUI : PreviewBlockUI
    {
        public RarityPreviewBlock Source
        {
            get => (RarityPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
            }
        }
    }

    public class GroupingPreviewBlockUI : PreviewBlockUI
    {
        public GroupingPreviewBlock Source
        {
            get => (GroupingPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                NameLists  = new ObservableCollection<string>();
                DataLists  = new ObservableCollection<IPreviewBlockData>();

                foreach (var data in value.DataLists)
                {
                    NameLists.Add(data.Name);
                    DataLists.Add(data);
                }
            }
        }

        public ObservableCollection<string> NameLists { get; private init; }
        public ObservableCollection<IPreviewBlockData> DataLists { get; private init; }
    }

    public class HistogramPreviewBlockUI : PreviewBlockUI
    {
        public HistogramPreviewBlock Source
        {
            get => (HistogramPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Color      = value.Color ?? "#007ACC";
                Value      = value.Value ?? new List<int>();
                Axis       = value.Axis ?? new List<string>();
            }
        }


        public string Color { get; init; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }

    public class RadarPreviewBlockUI : PreviewBlockUI
    {
        public RadarPreviewBlock Source
        {
            get => (RadarPreviewBlock)BaseSource;
            init
            {
                BaseSource = value;
                Color      = value.Color ?? "#007ACC";
                Value      = value.Value ?? new List<int>();
                Axis       = value.Axis ?? new List<string>();
            }
        }


        public string Color { get; init; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }
}