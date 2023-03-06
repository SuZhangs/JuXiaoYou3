using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public static class Constants
    {
        public static readonly DocumentType[] DocumentTypes = new[]
        {
            DocumentType.CharacterConstraint,
            DocumentType.GeographyConstraint,
            DocumentType.AbilityConstraint,
            DocumentType.ItemConstraint,
            DocumentType.OtherConstraint
        };
    }
}