using System.Collections.Generic;
using System.Drawing.Imaging;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class DataPartEditable<TCache, TDocument> : KeywordEditable<TCache, TDocument>
        where TDocument : class, IDataPartPackage
        where TCache : class, IDataCache
    {
        
        protected readonly Dictionary<string, DataPart> DataPartTrackerOfId;
        protected readonly Dictionary<Type, DataPart>   DataPartTrackerOfType;

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
            for (var i = 0; i < document.Parts.Count; i ++)
            {
                var part = document.Parts[i];
                if (!string.IsNullOrEmpty(part.Id) &&
                    !DataPartTrackerOfId.TryAdd(part.Id, part))
                {
                    document.Parts.RemoveAt(i);
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