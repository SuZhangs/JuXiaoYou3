namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    public class MetadataProcessor
    {
        
        private const char   ListSeparator     = '-';
        private const string NumberBasePattern =  "{0},{1},{2}";
        private const string ChartBasePattern  =  "{0},{1},{2},{3},{4},{5}";
        
        
        // BinaryBase : Binary
        // BinaryBase-Number : Degree
        // NumberBase : Number Slider Rate Likability
        // ChartBase : Radar Histogram
        // ReferenceBase : Audio Music Video File Image Reference
        // 
        private static string Combine(IEnumerable<string> values)
        {
            using var iterator = values.GetEnumerator();
            var       sb       = Pool.GetStringBuilder();

            if (!iterator.MoveNext())
            {
                return string.Empty;
            }
            
            sb.Append(iterator.Current);

            while (iterator.MoveNext())
            {
                sb.Append(ListSeparator);
                sb.Append(iterator.Current);
            }

            var v = sb.ToString();
            sb.Clear();
            Pool.ReturnStringBuilder(sb);
            return v;
        }
        public static string ChartBaseFormatted(
            IEnumerable<string> axis, 
            IEnumerable<int> value, 
            IEnumerable<int> fallback, 
            int maximum,
            int minimum,
            string color)
        {
            var a = Combine(axis);
            var b = Combine(value.Select(x => x.ToString()));
            var c = Combine(fallback.Select(x => x.ToString()));

            return string.Format(ChartBasePattern, maximum, minimum, color, a, b, c);
        }

        public static string NumberBaseFormatted(int max, int min, int value)
        {
            return string.Format(NumberBasePattern, max, min, value);
        }
    }
}