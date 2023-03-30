using System.Threading;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        [NullCheck(UniTestLifetime.Constructor)] private readonly object                    _sync;
        private readonly                                          Dictionary<int, Metadata> _MetadataTrackerByIndex;
        private readonly                                          Dictionary<string, int>   _MetadataTrackerByName;
        private                                                   int                       _currentIndex;
        
        protected void OnModuleBlockValueChanged(ModuleBlockDataUI dataUI, ModuleBlock block)
        {
            var metadataString = block.Metadata;
            
            //
            // ModuleBlockDataUI 已经实现了Value的Clamp和Fallback，不需要重新设置了
            // 这里只做Metadata的Add or Update
            
            if (string.IsNullOrEmpty(metadataString))
            {
                return;
            }
            
            //
            //
            var metadata = dataUI.ExtractMetadata();
            AddMetadata(metadata);
            
        }

        private void AddMetadata(Metadata metadata)
        {
            lock (_sync)
            {

                if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var metadataIndex))
                {
                    _document.Metas[metadataIndex].Value = metadata.Value;
                }
                else
                {
                    //
                    // 自增
                    Interlocked.Increment(ref _currentIndex);

                    var checkedIndex = _document.Metas.Count;
                    _document.Metas.Add(metadata);

                    if (checkedIndex != _currentIndex)
                    {
                        throw new InvalidOperationException("thread-not safe");
                    }
                    
                    _MetadataTrackerByIndex.Add(_currentIndex, metadata);
                    _MetadataTrackerByName.Add(metadata.Name, _currentIndex);
                }
            }
        }
    }
}