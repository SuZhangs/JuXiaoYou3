using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Binding;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class UniverseEditorViewModelProxy : BindingProxy<UniverseEditorBase>
    {
    }

    public abstract class UniverseEditorBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<SubViewBase> collection, string id) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name = Language.GetText(id),
                Type = typeof(TView)
            });
        }

        protected sealed override FrameworkElement CreateDetailPartView(IPartOfDetail part)
        {
            if (part is PartOfAlbum album)
            {
                return new AlbumPartView
                {
                    DataContext = new AlbumPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = album,
                        Document        = Document,
                        DocumentCache   = Cache,
                    }
                };
            }

            if (part is DetailPartSettingPlaceHolder)
            {
                return new DetailPartSettingView
                {
                    DataContext = this
                };
            }

            return null;
        }

        protected sealed override void OnSubViewChanged(SubViewBase oldValue, SubViewBase newValue)
        {
            if (newValue is not HeaderedSubView subView)
            {
                return;
            }

            subView.Create(this);
            SubView = subView.SubView;
        }
    }
}