﻿namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
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

        private static int[] Parse(string value)
        {
            return value.Split(ListSeparator)
                        .Select(x => int.TryParse(x, out var n) ? n : 10)
                        .ToArray();
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

        public static void ExtractChartBaseFormatted(
            string raw,
            out string[] axis,
            out int[] value,
            out int[] fallback,
            out int maximum,
            out int minimum,
            out string color)
        {
            var values = raw.Split(',');
            axis     = values[0].Split(',').
                                 ToArray();
            value    = Parse(values[1]);
            fallback = Parse(values[2]);
            maximum  = int.TryParse(values[3], out var n) ? n : 10;
            minimum  = int.TryParse(values[4], out  n) ? n : 0;
            color    = values[5];
        }

        public static string NumberBaseFormatted(int max, int min, int value)
        {
            return string.Format(NumberBasePattern, max, min, value);
        }
    }
}