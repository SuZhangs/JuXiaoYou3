namespace Acorisoft.FutureGL.MigaDB.IO
{

    public class Resource
    {
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
        
        /// <summary>
        /// 获得绝对路径
        /// </summary>
        /// <param name="path">基础地址</param>
        /// <returns>返回绝对路径</returns>
        public string GetAbsolutePath(string path)
        {
            return Path.Combine(path, RelativePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetUri()
        {
            throw new NotSupportedException();
        }
        
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; init; }
    }
}