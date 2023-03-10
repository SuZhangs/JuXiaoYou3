using System.Text;

namespace Acorisoft.FutureGL.MigaDB.IO
{
    public class Resource
    {
        public const            string AvatarUriPattern = "avatar_{0}";
        public const            string UriPattern = "pos_{0}:{1}";
        private static readonly byte[] Prefix           = Encoding.ASCII.GetBytes("pos_");
        
        public static string ParseOldVersion(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return null;
            }

            // miga://v1.img/param
            return src.Length < 14 ? null : ParseOldVersionImpl(src);
        }

        private static string ParseOldVersionImpl(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return null;
            }

            // miga://v1.img/param
            if (src.Length < 14)
            {
                return null;
            }

            var schemeName      = src[..4];
            var schemeVerPrefix = src[7];
            var schemeVer       = int.TryParse(src.AsSpan(8, 1), out var val) ? val : 0;
            var param           = src[14..];

            // ReSharper disable once MergeIntoLogicalPattern
            if (schemeVerPrefix != 'v' && schemeVerPrefix != 'V')
            {
                return null;
            }

            if (schemeVer is < 0 or > 3)
            {
                return null;
            }


            return schemeName.EqualsWithIgnoreCase("miga") ? $"resx.v100.{param}" : null;
        }

        private static byte GetFlags(ResourceType type)
        {
            return type switch
            {
                ResourceType.Video => 0x76, // v
                ResourceType.Audio => 0x61, // a
                ResourceType.File  => 0x66, // f
                ResourceType.Music => 0x6d, //m
                _                  => 0x69  // i
            };
        }

        public static string ToUnifiedUri(string id, ResourceType type)
        {
            // pos_i:{id}
            return string.Format(UriPattern, GetFlags(type), id);
        }
        
        public static string ToAvatarUri(string id, ResourceType type)
        {
            // pos_i:{id}
            return string.Format(UriPattern, GetFlags(type), string.Format(AvatarUriPattern, id));
        }
    }
}