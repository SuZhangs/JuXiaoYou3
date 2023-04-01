using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
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

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        private readonly Dictionary<string, DataPart> _DataPartTrackerOfId;

        
        private bool AddModule(PartOfModule module)
        {
            if (module is null)
            {
                return false;
            }
            
            if (_DataPartTrackerOfId.TryAdd(module.Id, module))
            {
                _document.Parts.Add(module);
                ModuleParts.Add(module);
                
                foreach (var block in module.Blocks
                                            .Where(x => !string.IsNullOrEmpty(x.Metadata)))
                {
                    AddMetadata(block.ExtractMetadata());
                }
                return true;
            }

            return false;
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
                else if (part is IPartOfDetail custom)
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