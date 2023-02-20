namespace Acorisoft.FutureGL.MigaDB
{
    public class Constants
    {
        /*
         * Database Const Fields : string
         * 数据库常量字段
         */
        internal const string DatabaseFileName      = "main.mgdb";
        internal const string DatabaseIndexFileName = "index.mgidx";


        /*
         * Database Const Fields : Int
         * 数据库常量字段（非字符串）
         */
        internal const int DatabaseSize = 8 * 1048576;
        internal const int MinVersion   = 0;
        internal const int MaxVersion   = 512;


        /*
         * Database Engine Fields
         * 数据库引擎字段
         */
        internal const string PropertyCollectionName    = "_props";
        internal const string Name_ModuleTemplate       = "cn_module";
        internal const string Name_Cache_ModuleTemplate = "cn_cache_module";
        internal const string Name_Cache_Metadata       = "cn_cache_meta";


        /*
         * Folder Name
         * 文件夹名字
         */
        internal const string ImageFolderName = "Image";
        internal const string MusicFolderName = "Music";
        internal const string AudioFolderName = "Audio";

        /*
         * LiteDB Fields
         * 数据库字段
         */
        public const string LiteDB_IdField   = "_id";
        public const string LiteDB_NameField = "name";

        /*
         * DatabaseVersion
         * 当前数据库版本
         */
        public const int DatabaseCurrentVersion = 0;
    }
}