using Acorisoft.FutureGL.MigaDB.Data.Concepts;
// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            Knowledge k;
            
            if (GetElementalKnowledge(id, out k))
            {
                return k;
            }

            if (GetTechnologyKnowledge(id, out k))
            {
                return k;
            }
            
            if (GetFaithKnowledge(id, out k))
            {
                return k;
            }

            return null;
        }

        protected sealed override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            
            ElementalOpening(database);           
            FaithOpening(database);
            TechnologyOpening(database);
        }

        protected sealed override void OnDatabaseClosingOverride()
        {
            ElementalClosing();
            FaithClosing();
            TechnologyClosing();
        }
    }
}