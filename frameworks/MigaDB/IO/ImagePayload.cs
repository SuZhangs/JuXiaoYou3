namespace Acorisoft.FutureGL.MigaDB.IO
{
    public class ImagePayload
    {
        public string Payload { get; init; }
        public byte[] ImageData { get; init; }
    }
    
    public class ImagePayload<T>
    {
        public T Value { get; init; }
        public byte[] ImageData { get; init; }
    }
}