namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public enum DocumentType : int
    {
        None       = 0,
        Document   = 0x1000,
        Character  = Document + 0x001,
        Skill    = Document + 0x002,
        Geography  = Document + 0x004,
        NPC,
        Creature,
        Plant,
        Resource,
        Weapon,
        Armor,
        Costume,
        Medicine,
        Item,
        Material,
        Ore,
        Country,
        Gangbang,
        Team,
        God,
        Devil,
        Poison,
        Calamity,
        Elemental,
        Technology,
        Magic,
        Physical,
        Other,
    }
}