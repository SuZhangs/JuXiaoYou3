namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public abstract class DataPartEditable<TCache, TDocument> : KeywordEditable<TCache, TDocument>
        where TDocument : class, IDataPartPackage
        where TCache : class, IDataCache
    {
        
        private protected readonly Dictionary<string, DataPart> DataPartTrackerOfId;
        private protected readonly Dictionary<Type, DataPart>   DataPartTrackerOfType;

        protected DataPartEditable()
        {
            DataPartTrackerOfId   = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);
            DataPartTrackerOfType = new Dictionary<Type, DataPart>();
        }
    }
}