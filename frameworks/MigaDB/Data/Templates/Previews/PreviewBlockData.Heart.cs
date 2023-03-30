namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewHeartData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Clamp to 0-5
        /// </summary>
        public int Value { get; init; }
    }
}