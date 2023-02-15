namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    public enum MetadataKind : int
    {
        /*
         * Basic
         */
        BasicType = 0x00010000,
        Color     = BasicType + 0x0001,
        Degree    = BasicType + 0x0002,
        Number    = BasicType + 0x0004,
        Slider    = BasicType + 0x0008,
        Text      = BasicType + 0x000f,
        Page      = BasicType + 0x0010,

        /*
         * Option
         */
        OptionType = 0x00020000,
        Switch     = OptionType + 0x0001,
        Radio      = OptionType + 0x0002,
        Talent     = OptionType + 0x0004,
        Favorite   = OptionType + 0x0008,
        Binary     = OptionType + 0x000f,
        Sequence   = OptionType + 0x0010,

        /*
        * Reference
        */
        ReferenceType = 0x00040000,
        Reference     = ReferenceType + 0x0001,
        Image         = ReferenceType + 0x0002,
        Video         = ReferenceType + 0x0004,
        Music         = ReferenceType + 0x0008,
        Audio         = ReferenceType + 0x000F,
        File          = ReferenceType + 0x0010,

        /*
         * Chart
         */
        ChartType = 0x00080000,
        Histogram = ChartType + 0x0001,
        Radar     = ChartType + 0x0002,

        /*
         * Group
         */
        GroupType  = 0x000F0000,
        Likability = GroupType + 0x0001,
        Rate       = GroupType + 0x0002,
        Group      = GroupType + 0x0004,
    }
}