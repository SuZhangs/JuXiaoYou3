using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class RelationshipProperty : ObservableObject
    {
        public ObservableCollection<RelationshipDefinition> Definitions { get; init; }
    }
}