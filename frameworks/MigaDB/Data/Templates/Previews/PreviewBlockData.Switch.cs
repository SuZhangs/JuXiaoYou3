namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Previews
{
    public class PreviewSwitchData : ObservableObject, IPreviewBlockData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Clamp to 0-5
        /// </summary>
        public bool Value { get; init; }
    }
}