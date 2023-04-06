using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public abstract class PreviewBlock : StorageUIObject
    {
        private string _name;
        public int Index { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public bool IsMetadata { get; init; }
        public MetadataKind Type { get; init; }
    }

    public sealed class RarityPreviewBlock : PreviewBlock
    {
        public double Scale { get; init; }
        public string ValueSourceID { get; set; }
    }

    public sealed class StringPreviewBlock : PreviewBlock
    {
        public string ValueSourceID { get; set; }
    }

    public sealed class GroupingPreviewBlock : PreviewBlock
    {
        public ObservableCollection<IPreviewBlockData> DataLists { get; init; }
    }

    public sealed class ChartPreviewBlock : PreviewBlock
    {
        public string ValueSourceID { get; set; }
        public ChartType ChartType { get; init; }
    }
}