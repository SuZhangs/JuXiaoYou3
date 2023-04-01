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
            private          int          _refCount     = 0;
            private readonly HashSet<int> _blockTracker = new HashSet<int>();

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

            public bool Remove(int hashCode, IList<Metadata> metadataCollection)
            {
                if (_blockTracker.Remove(hashCode))
                {
                    _refCount--;
                }

                if (_refCount == 0)
                {
                    metadataCollection.RemoveAt(_index);
                    _blockTracker.Clear();
                    return true;
                }

                return false;
            }

            public string this[MetadataCollection metadataCollection, int hashCode]
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

                    if (_blockTracker.Add(hashCode))
                    {
                        _refCount++;
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
            
            AddMetadata(metadata, block);
            
        }
        
        private void AddMetadata(Metadata metadata)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var metadataIndex))
            {
                var hashCode = HashCode.Combine(metadata.Name);
                metadataIndex[_document.Metas, hashCode] = metadata.Value;
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

        private void AddMetadata(Metadata metadata, ModuleBlock block)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var metadataIndex))
            {
                var hashCode = HashCode.Combine(block, metadata.Name);
                metadataIndex[_document.Metas, hashCode] = metadata.Value;
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
            if (!_MetadataTrackerByName.TryGetValue(metadata.Name, out var index))
            {
                return;
            }
            
            var hashCode = HashCode.Combine(metadata.Name);
            if (index.Remove(hashCode, _document.Metas))
            {
                _MetadataTrackerByName.Remove(metadata.Name);
            }
        }

        private void RemoveMetadata(Metadata metadata, ModuleBlock block)
        {
            if (!_MetadataTrackerByName.TryGetValue(metadata.Name, out var index))
            {
                return;
            }
            
            var hashCode = HashCode.Combine(block, metadata.Name);
            if (index.Remove(hashCode, _document.Metas))
            {
                _MetadataTrackerByName.Remove(metadata.Name);
            }
        }

        private void ClearMetadata()
        {
            _MetadataTrackerByName.Clear();
            _document.Metas.Clear();
        }
    }
}