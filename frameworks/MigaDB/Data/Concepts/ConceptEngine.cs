using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Data.Concepts
{
    // TODO: 添加DataEngine到DataEngineType
    // 记得实现 Concept的聚合（Aggregated）
    public class ConceptEngine : DataEngine
    {
        public bool HasConcept(Concept concept)
        {
            return concept is not null &&
                   ConceptDB is not null &&
                   ConceptDB.HasID(concept.Id);
        }

        public void AddConcept(Concept concept)
        {
            if (concept is null ||
                ConceptDB is null)
            {
                return;
            }

            ConceptDB.Upsert(concept);
        }
        
        public void UpdateConcept(string id, string name)
        {
            if (string.IsNullOrEmpty(id)||
                string.IsNullOrEmpty(name) ||
                ConceptDB is null)
            {
                return;
            }

            var concept =ConceptDB.FindById(id);
            if (concept is null)
            {
                return;
            }

            concept.Name = name;
            ConceptDB.Update(concept);
        }
        
        public void RemoveConcept(Concept concept)
        {
            if (concept is null ||
                ConceptDB is null)
            {
                return;
            }

            ConceptDB.Delete(concept.Id);
        }
        
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            ConceptDB = session.Database.GetCollection<Concept>(Constants.Name_Concept);
        }

        protected override void OnDatabaseClosing()
        {
            ConceptDB = null;
        }
        
        public ILiteCollection<Concept> ConceptDB { get; private set; }
    }
}