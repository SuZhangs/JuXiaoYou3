using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public static class Constants
    {
        public static readonly DocumentType[] DocumentTypes = new[]
        {
            DocumentType.CharacterDocument,
            DocumentType.GeographyDocument,
            DocumentType.AbilityDocument,
            DocumentType.ItemDocument,
            DocumentType.OtherDocument
        };
    }
}