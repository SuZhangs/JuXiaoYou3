using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public abstract class PreviewBlock : StorageUIObject
    {
        private string _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }

    public sealed class RarityPreviewBlock : PreviewBlock
    {
        
    }

    public sealed class GroupingPreviewBlock : PreviewBlock
    {
        public ObservableCollection<IPreviewBlockData> DataLists { get; init; }
    }
    
    public sealed class HistogramPreviewBlock : PreviewBlock
    {
        public string Color { get; init; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }
    
    public sealed class RadarPreviewBlock : PreviewBlock
    {
        public string Color { get; init; }
        public List<string> Axis { get; init; }
        public List<int> Value { get; init; }
    }
}