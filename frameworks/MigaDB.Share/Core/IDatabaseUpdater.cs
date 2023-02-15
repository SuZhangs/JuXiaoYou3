namespace Acorisoft.FutureGL.MigaDB.Core
{
    public interface IDatabaseUpdater
    {
        int Update(IDatabase database);
        int TargetVersion { get; }
        int ResultVersion { get; }
    }
}