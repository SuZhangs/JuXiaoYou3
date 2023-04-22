using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Core.Migrations
{
    public class UpdaterOfVer100 : DatabaseUpdater
    {
        protected override void Execute(IDatabase database)
        {

        }

        private static void ExecuteUpdate_RelationshipDefinition(IDatabase database)
        {

        }

        public override int TargetVersion => 0;
        
        public override int ResultVersion => 100;
    }
}