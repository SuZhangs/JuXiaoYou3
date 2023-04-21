using Acorisoft.FutureGL.MigaDB.Data;

namespace Acorisoft.FutureGL.MigaDB.Maintainers
{
    public class RelationshipMaintainer : DatabaseMaintainer<RelationshipProperty>
    {
        private static 
        
        protected override RelationshipProperty OnCreateInstance()
        {
            throw new NotImplementedException();
        }

        protected override void OnFixInstance(RelationshipProperty instance)
        {
            throw new NotImplementedException();
        }

        protected override bool IsInvalidated(RelationshipProperty instance)
        {
            return instance?.Definitions is null ||
                   instance.Definitions.Count == 0;
        }
    }
}