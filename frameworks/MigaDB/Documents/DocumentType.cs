namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public enum DocumentType : int
    {
        Constraint                = 0x0100,
        CharacterConstraint       = Constraint + 0x001,
        AbilityConstraint         = Constraint + 0x002,
        GeographyConstraint       = Constraint + 0x004,
        ItemConstraint            = Constraint + 0x008,
        OtherConstraint           = Constraint + 0x010,
        FantasyUniverseConstraint = Constraint + 0x020,
        Compose,
    }
}