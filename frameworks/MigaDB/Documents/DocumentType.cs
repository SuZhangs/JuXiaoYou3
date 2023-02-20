namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public enum DocumentType : int
    {
        Rule                = 0x0100,
        CharacterRule       = Rule + 0x001,
        AbilityRule         = Rule + 0x002,
        GeographyRule       = Rule + 0x004,
        ItemRule            = Rule + 0x008,
        OtherRule           = Rule + 0x010,
        FantasyUniverseRule = Rule + 0x020,
        Compose,
    }
}