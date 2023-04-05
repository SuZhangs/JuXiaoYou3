using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class DocumentEditorVMBase
    {
        private class MetadataIndexCache
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
        
        private readonly Dictionary<string, DataPart> _DataPartTrackerOfId;
        private readonly Dictionary<Type, DataPart>   _DataPartTrackerOfType;

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
        
        private void RemoveMetadata(string metadata)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata, out var cache))
            {
                _MetadataTrackerByName.Remove(metadata);
                _document.Metas.RemoveAt(cache.Index);
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


        private PartOfModule GetModuleById(string id)
        {
            return _DataPartTrackerOfId.TryGetValue(id, out var module) ? (PartOfModule)module : null;
        }
        
        //
        // DocumentManager Part
        protected void OpenDocument(Document document)
        {
            //
            // Clear
            SelectedDetailPart = null;
            ModuleParts.Clear();
            InvisibleDataParts.Clear();

            foreach (var part in document.Parts)
            {
                if (part is PartOfModule module)
                {
                    ModuleParts.Add(module);
                }
                else if (part is PartOfDetail custom)
                {
                    DetailParts.Add(custom);
                }
                else
                {
                    InvisibleDataParts.Add(part);
                }
            }
        }
    }
}