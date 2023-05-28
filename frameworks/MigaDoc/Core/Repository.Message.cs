namespace Acorisoft.Miga.Doc.Core
{

    public class RepoOpenMessage : INotification
    {
        public RepositoryContext Context { get; init; }
        public RepositoryProperty Property { get; init; }
    }

    public class RepoSetMessage : INotification
    {
        public UserCredential Credential { get; init; }
    }
    
    public class RepoCloseMessage : INotification
    {
        public RepositoryContext Context { get; init; }
    }
}