using Acorisoft.FutureGL.MigaDB.Data.Relationships;
using GraphShape.Controls;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class CharacterGraph : BidirectionalGraph<DocumentCache, CharacterRelationship>
    {
        
    }

    public class CharacterGraphLayout : GraphLayout<DocumentCache, CharacterRelationship, CharacterGraph>
    {
        
    }
}