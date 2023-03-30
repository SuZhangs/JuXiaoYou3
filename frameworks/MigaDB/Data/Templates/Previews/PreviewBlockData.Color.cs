namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewColorData :ObservableObject,  IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Clamp to 0-5
        /// </summary>
        public string Value { get; init; }
    }
}