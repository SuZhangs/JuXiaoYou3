using Acorisoft.FutureGL.MigaDB.Data.Universe;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    public class ElementalRelationship: StorageUIObject, IEdge<Elemental>
    {
        public Elemental Source { get; }
        public Elemental Target { get; }
    }
}