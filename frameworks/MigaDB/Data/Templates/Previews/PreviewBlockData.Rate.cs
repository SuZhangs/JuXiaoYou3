namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewRateData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        public double Scale { get; init; }
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
    
    public class PreviewLikabilityData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        public double Scale { get; init; }
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
}