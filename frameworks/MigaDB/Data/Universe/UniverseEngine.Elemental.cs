using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine
    {
        protected bool GetElementalKnowledge(string id, out Knowledge knowledge)
        {
            throw new NotImplementedException();
        }
        
        protected void ElementalOpening(IDatabase database)
        {
            ElementalDB    = database.GetCollection<Elemental>(Constants.Name_Elemental);
            ElementalRelDB = database.GetCollection<ElementalRelationship>(Constants.Name_Relationship_Elemental);
        }

        protected void ElementalClosing()
        {
            ElementalDB     = null;
            ElementalRelDB  = null;
        }

        public ILiteCollection<Elemental> ElementalDB { get; private set; }
        public ILiteCollection<ElementalRelationship> ElementalRelDB { get; private set; }
    }
}