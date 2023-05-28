namespace Acorisoft.Miga.Doc.Parts
{
    /// <summary>
    /// <see cref="CustomDataPart"/> 类型表示一个自定义部件集合。
    /// </summary>
    public class CustomDataPart : DataPart
    {

        public sealed override void Initialized(IMetadataManager metadataManager)
        {
            foreach (var property in Properties)
            {
                if (property is IFallbackSupport fallbackSupport && string.IsNullOrEmpty(property.Value))
                {
                    property.Value = fallbackSupport.Fallback;
                }
            }
            
            base.Initialized(metadataManager);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<InputProperty> Properties { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
    }
}