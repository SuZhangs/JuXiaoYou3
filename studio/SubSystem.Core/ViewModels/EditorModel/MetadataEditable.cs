using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class MetadataEditable<TCache, TDocument> : DataPartEditable<TCache, TDocument>
        where TDocument : class, IMetadataPackage
        where TCache : class, IDataCache
    {
        protected MetadataEditable()
        {
            MetadataTrackerByName = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
        }
        
        /*
         * MetadataIndexCache would tracking the metadata in document metadata collection
         * so,
         */
        
        [NullCheck(UniTestLifetime.Constructor)]
        protected readonly Dictionary<string, Metadata> MetadataTrackerByName;
        
        
        protected class MetadataIndexCache
        {
            private int _index;

            public Metadata Source { get; init; }

            public int Index
            {
                get => _index;
                init => _index = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public string Value => Source.Value;

            public string this[MetadataCollection metadataCollection]
            {
                set
                {
                    if (Index < 0 || Index > metadataCollection.Count - 1)
                    {
                        _index = metadataCollection.Count;
                        metadataCollection.Add(Source);
                        Source.Value = value;
                    }
                    else
                    {
                        var outside = metadataCollection[Index];

                        if (outside.Value != Source.Value)
                        {
                            metadataCollection[Index].Value = value;
                        }

                        Source.Value = value;
                    }
                }
            }
        }

        #region Metadata

        

        protected Metadata GetMetadataById(string metadata)
        {
            return MetadataTrackerByName.TryGetValue(metadata, out var meta) ? meta : null;
        }
        
        protected void AddMetadata(Metadata metadata)
        {
            if(metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }

            if (MetadataTrackerByName.TryGetValue(metadata.Name, out var insideMetadata))
            {
                insideMetadata.Value = metadata.Value;
            }
            else
            {
                insideMetadata = Document.Metas
                                         .Find(x => x.Name == metadata.Name);
                if (insideMetadata is null)
                {
                    insideMetadata = metadata;
                    Document.Metas.Add(insideMetadata);
                }
                else
                {
                    insideMetadata.Value = metadata.Value;
                }
                
                MetadataTrackerByName.Add(metadata.Name, metadata);
            }
        }
        
        protected void RemoveMetadata(string metadata)
        {
            if (MetadataTrackerByName.TryGetValue(metadata, out var cache))
            {
                //
                // the problem is that when we remove a cache from document
                // the cache position will be changed
                // 
                Document.Metas
                        .Remove(cache);
                MetadataTrackerByName.Remove(metadata);
            }
        }

        protected void RemoveMetadata(Metadata metadata)
        {
            if (metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }
            RemoveMetadata(metadata.Name);
        }

        protected void ClearMetadata()
        {
            MetadataTrackerByName.Clear();
            Document.Metas.Clear();
        }


        #endregion
    }
}