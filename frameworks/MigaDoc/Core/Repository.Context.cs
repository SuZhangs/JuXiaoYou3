namespace Acorisoft.Miga.Doc.Core
{
    public class RepositoryContext
    {
        public ILiteDatabase Database { get; init; }
        public string RepositoryFolder { get; init; }
        public RepositoryInformation RepositoryInformation { get; init; }
    }
}