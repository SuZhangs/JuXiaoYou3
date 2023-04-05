using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public static class Constants
    {
        public static readonly DocumentType[] DocumentTypes = new[]
        {
            DocumentType.Character,
            DocumentType.Geography,
            DocumentType.Ability,
            DocumentType.Item,
            DocumentType.Other
        };
    }
}