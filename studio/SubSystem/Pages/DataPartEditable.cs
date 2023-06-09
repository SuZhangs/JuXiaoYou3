using System.Drawing.Imaging;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages
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

        protected void AddDataPart(DataPart dataPart)
        {
            if (DataPartTrackerOfId.TryAdd(dataPart.Id, dataPart))
            {
                DataPartTrackerOfType.TryAdd(dataPart.GetType(), dataPart);
            }
        }

        protected void ClearDataPart()
        {
            DataPartTrackerOfId.Clear();
            DataPartTrackerOfId.Clear();
        }

        protected void AddDataPart(TDocument document)
        {
            var logger = Xaml.Get<ILogger>();
            foreach (var part in document.Parts)
            {
                if (!string.IsNullOrEmpty(part.Id) &&
                    !DataPartTrackerOfId.TryAdd(part.Id, part))
                {
                    logger.Warn($"部件没有ID或者部件重复不予添加，部件ID：{part.Id}");
                    continue;
                }

                if (OnDataPartAddingBefore(part))
                {
                    continue;
                }
                
                OnDataPartAddingAfter(part);
            }
            
        }
        
        
        protected sealed override void LoadDocument(TCache cache, TDocument document)
        {
            AddDataPart(document);
            IsDataPartExistence(document);
        }

        protected abstract bool OnDataPartAddingBefore(DataPart part);
        protected abstract void OnDataPartAddingAfter(DataPart part);
        protected abstract void IsDataPartExistence(TDocument document);
    }
}