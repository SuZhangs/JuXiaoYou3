using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine
    {
        protected bool GetTechnologyKnowledge(string id, out Knowledge knowledge)
        {
            throw new NotImplementedException();
        }
        
        protected void TechnologyOpening(IDatabase database)
        {
            TechnologyDB    = database.GetCollection<Technology>(Constants.Name_Technology);
            TechnologyRelDB = database.GetCollection<TechnologyRelationship>(Constants.Name_Relationship_Technology);
        }

        protected void TechnologyClosing()
        {
            TechnologyDB    = null;
            TechnologyRelDB = null;
        }

        public ILiteCollection<Technology> TechnologyDB { get; private set; }
        public ILiteCollection<TechnologyRelationship> TechnologyRelDB { get; private set; }
    }
}