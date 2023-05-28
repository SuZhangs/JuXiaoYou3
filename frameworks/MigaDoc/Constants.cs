namespace Acorisoft.Miga.Doc
{
    public enum EntityID
    {
        Channel,
        ChannelIndex,
        ComposeIndex,
        DocumentIndex,
        Document,
        Group,
        Keyword,
        Sight,
        KeywordMapping,
        Moniker,
        Organization,
        Relationship,
        Conversation,
        Timeline
    }
    
    public static class Constants
    {
        //
        // fileName
        internal const string main_database = "main.mgdb";
        internal const string index_file    = "index.migaidx";

        //
        // collection name
        internal const string cn_modules          = "mods";
        internal const string cn_meow             = "meow";
        internal const string cn_prop             = "props";
        internal const string cn_index            = "idx";
        internal const string cn_index_compose    = "idx_compose";
        internal const string cn_index_channel    = "idx_ch";
        internal const string cn_document         = "doc";
        internal const string cn_channel          = "ch";
        internal const string cn_message          = "msgs";
        internal const string cn_inspiration      = "ins";
        internal const string cn_compose          = "compose";
        internal const string cn_keyword          = "tags";
        internal const string cn_sight            = "sights";
        internal const string cn_keywordMapping   = "rel_tags";
        internal const string cn_characterMapping = "rel_ch";
        internal const string cn_org              = "org";
        internal const string cn_group            = "group";
        internal const string cn_moniker          = "monikers";

        //
        // folder name
        internal const string folder_modules = "Modules";

        //
        // field
        internal const string fieldName_id = "_id";

        internal static T Deserialize<T>(this BsonDocument document)
        {
            return BsonMapper.Global.Deserialize<T>(document);
        }
    }
}