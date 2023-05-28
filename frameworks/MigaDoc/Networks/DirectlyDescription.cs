namespace Acorisoft.Miga.Doc.Networks
{
    public enum ObjectState
    {
        Original,
        Added,
        Removed
    }
    
    public class DirectlyDescription
    {
        public string Id { get; init; }
        public EntityID EntityId { get; init; }
    }

    public class DirectlyTransaction : Transaction
    {
        public string Id { get; init; }
        public ObjectState State { get; init; }
        public EntityID EntityId { get; init; }
    }
}