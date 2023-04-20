using Acorisoft.FutureGL.MigaDB.Data.Concepts;

namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            // ReSharper disable once InlineOutVariableDeclaration
            Knowledge k;
            
            if (GetElementalKnowledge(id, out k))
            {
                return k;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (GetTechnologyKnowledge(id, out k))
            {
                return k;
            }

            return null;
        }

        protected sealed override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            ElementalOpening(database);
            TechnologyOpening(database);
        }

        protected sealed override void OnDatabaseClosingOverride()
        {
            ElementalClosing();
            TechnologyClosing();
        }
    }
}