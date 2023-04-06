namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewTextData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
}