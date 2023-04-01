using System.Collections;
using System.Threading;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.Miga.Doc.Parts;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        private class MetadataIndexCache
        {
            private          int          _index;

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
                        Source.Value =  value;
                    }
                    else
                    {
                        var outside = metadataCollection[Index];

                        if (outside.Value != Source.Value)
                        {
                            metadataCollection[Index].Value = value;
                        }

                        Source.Value =  value;
                    }
                }
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)] private readonly object                                 _sync;
        [NullCheck(UniTestLifetime.Constructor)] private readonly Dictionary<string, MetadataIndexCache> _MetadataTrackerByName;

        private int _currentIndex;
        
        protected void OnModuleBlockValueChanged(ModuleBlockDataUI dataUI, ModuleBlock block)
        {
            var metadataString = block.Metadata;
            
            //
            // ModuleBlockDataUI 已经实现了Value的Clamp和Fallback，不需要重新设置了
            // 这里只做Metadata的Add or Update
            if (block is GroupBlock)
            {
                return;
            }
            
            if (string.IsNullOrEmpty(metadataString))
            {
                return;
            }
            
            //
            //
            var metadata = block.ExtractMetadata();
            
            AddMetadata(metadata);
            
        }
        
        private void AddMetadata(Metadata metadata)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var metadataIndex))
            {
                metadataIndex[_document.Metas] = metadata.Value;
            }
            else
            {

                var checkedIndex = _document.Metas.Count;
                
                _document.Metas.Add(metadata);

                if (checkedIndex != _currentIndex)
                {
                    throw new InvalidOperationException("thread-not safe");
                }
                
                var index = new MetadataIndexCache
                {
                    Source = metadata,
                    Index  = checkedIndex
                };
                
                _MetadataTrackerByName.Add(metadata.Name, index);
                    
                //
                // 自增
                Interlocked.Increment(ref _currentIndex);
            }
        }

        private void RemoveMetadata(Metadata metadata)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var cache))
            {
                _MetadataTrackerByName.Remove(metadata.Name);
                _document.Metas.RemoveAt(cache.Index);
            }
        }

        private void ClearMetadata()
        {
            _MetadataTrackerByName.Clear();
            _document.Metas.Clear();
        }
    }
}