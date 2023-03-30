using System.Text;

namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    /// <summary>
    /// 元数据
    /// </summary>
    public class Metadata
    {
        private const char ListSeparator = ',';
        
        public static string CombineBinaryPattern(string negative, string positive, int maximum, int minimum)
        {
            var sb = Pool.GetStringBuilder();
            sb.Append(negative);
            sb.Append(ListSeparator);
            sb.Append(positive);
            sb.Append(ListSeparator);
            sb.Append(maximum);
            sb.Append(ListSeparator);
            sb.Append(minimum);
            sb.Append(ListSeparator);
            var result = sb.ToString();
            sb.Clear();
            Pool.ReturnStringBuilder(sb);
            return result;
        }
        
        // BinaryBase : Binary
        // BinaryBase-Number : Degree
        // NumberBase : Number Slider Rate Likability
        // ChartBase : Radar Histogram
        // ReferenceBase : Audio Music Video File Image Reference
        // 
        
        /// <summary>
        /// 元数据的名字
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 元数据的值
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// 元数据的额外参数
        /// </summary>
        /// <remarks>
        /// 用于记录恢复元数据所需的额外内容（例如雷达图的图例。）
        /// </remarks>
        public string Parameters { get; init; }
        
        /// <summary>
        /// 元数据的类型。
        /// </summary>
        public MetadataKind Type { get; init; }
    }
}