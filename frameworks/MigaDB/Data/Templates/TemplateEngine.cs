using Acorisoft.FutureGL.MigaDB.Services;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates
{
    public partial class TemplateEngine : DataEngine
    {
        public override void Refresh()
        {
        }

        public override void Clear()
        {
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            ModuleOpening(session);
        }

        protected override void OnDatabaseClosing()
        {
            ModuleClosing();
        }
    }
}