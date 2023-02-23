namespace Acorisoft.FutureGL.MigaDB.IO
{
    // v100
    // 其中第一位是图片参数位
    //
    // 1: 表示等比图像
    // 2: 表示横向图像，满足240P
    // 3: 表示横向图像，满足720P
    // 4: 表示横向图像，满足1080P
    // 5: 表示纵向图像，满足240P
    // 6: 表示纵向图像，满足720P
    // 7: 表示纵向图像，满足1080P

    [Flags]
    public enum ResourceProperty
    {
        Padding                  = 0x1,
        Image                    = 0x00100,
        Image_Square             = Image + 0x010,
        Image_Horizontal_MinSize = Image + 0x011,
        Image_Horizontal_240P    = Image + 0x010,
        Image_Horizontal_720P    = Image + 0x040,
        Image_Horizontal_1080P   = Image + 0x080,
        Image_Vertical_MinSize   = Image + 0x101,
        Image_Vertical_240P      = Image + 0x110,
        Image_Vertical_720P      = Image + 0x120,
        Image_Vertical_1080P     = Image + 0x140,
        File                     = 0x00300,
        SmallFile                = File + 0x010,
        MediumFile               = File + 0x012,
        LargeFile                = File + 0x014,
        Music                    = File + 0x018,
        Audio                    = File + 0x020,
        Video                    = File + 0x022,
    }

    public class Resource
    {
        private static string ParseOldVersion(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return null;
            }

            // miga://v1.img/param
            return src.Length < 14 ? null : ParseImpl(src);
        }

        private static string ParseImpl(string src)
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

            var schemeName = src[..4];
            var schemeVerPrefix = src[7];
            var schemeVer = int.TryParse(src.AsSpan(8, 1), out var val) ? val : 0;
            var param = src[14..];

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

        public static Resource Parse(string value)
        {
            // file.img.id
            // resx.v100.data
            // res.v100:data

            var schemaName = value[..3];
            var propertyFlagWithHeader = value.Substring(4, 4);
            var property = value.Substring(5, 3);
            var data = value[8..];

            if (schemaName.EqualsWithIgnoreCase("res") &&
                char.ToLower(propertyFlagWithHeader[0]) == 'v' &&
                property.All(char.IsDigit))
            {
                var propertyFlagValue = int.Parse(property);
                return new Resource
                {
                    RelativePath = data,
                    Property     = (ResourceProperty)propertyFlagValue
                };
            }

            var val = ParseOldVersion(value);

            if (string.IsNullOrEmpty(val))
            {
                return null;
            }

            return new Resource
            {
                Property     = ResourceProperty.Image,
                RelativePath = val
            };
        }

        public string RelativePath { get; init; }

        /// <summary>
        /// 获取当前资源的属性
        /// </summary>
        public ResourceProperty Property { get; init; }
    }
}