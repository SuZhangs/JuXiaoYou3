using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaDB.Migrations
{
    public class UpdaterOfVer100 : DatabaseUpdater
    {
        protected override void Execute(IDatabase database)
        {
            //
            //
            ExecuteUpdate_RelationshipDefinition(database);
        }

        private static void ExecuteUpdate_RelationshipDefinition(IDatabase database)
        {
            database.Upsert(new RelationshipProperty
            {
                Definitions = new ObservableCollection<RelationshipDefinition>()
            });
        }

        public override int TargetVersion => 100;
        
        public override int ResultVersion => 0;
    }
}