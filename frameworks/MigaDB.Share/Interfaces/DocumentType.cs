namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public enum DocumentType : int
    {
        None              = 0,
        Document          = 0x0100,
        CharacterDocument = Document + 0x001,
        AbilityDocument   = Document + 0x002,
        GeographyDocument = Document + 0x004,
        ItemDocument      = Document + 0x008,
        OtherDocument     = Document + 0x010,
        MysteryDocument   = Document + 0x020,
        Compose,
    }
}