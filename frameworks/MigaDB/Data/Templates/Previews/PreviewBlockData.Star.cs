namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewStarData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        public string Metadata { get; init; }
    }
}