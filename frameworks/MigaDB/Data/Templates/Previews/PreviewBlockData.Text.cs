namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewTextData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        
        public string Metadata { get; init; }
    }
}