namespace Acorisoft.FutureGL.MigaDB
{
    public static class Feature
    {
        public static LiteDatabase GetLiteDatabase(this IDatabase database)
        {
            return ((Database)database)._database;
        }
        
        public const string TextEditorFeatureMissing = "feature.TextEditor";
    }
}