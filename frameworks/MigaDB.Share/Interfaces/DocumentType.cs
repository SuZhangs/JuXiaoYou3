namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public enum DocumentType : int
    {
        None       = 0,
        Document   = 0x1000,
        Character  = Document + 0x001,
        Ability    = Document + 0x002,
        Geography  = Document + 0x004,
        Item       = Document + 0x008,
        Other      = Document + 0x010,
        Universe   = Document + 0x2000,
        World      = Universe + 0x100,
        God        = World + 0x001,
        Devil      = World + 0x002,
        NPC        = World + 0x004,
        Disaster   = World + 0x008,
        Poison     = World + 0x00f,
        Elemental  = World + 0x010,
        Technology = World + 0x011,
        Magic      = World + 0x012,
        Physical   = World + 0x014,
        Compose,
    }
}