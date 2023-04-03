﻿using System;
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
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
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
        private static IEnumerable<object> CreateDetailPart()
        {
            return new object[]
            {
                new PartOfAlbum(),
                new PartOfPlaylist(),
                new PartOfRel()
            };
        }

        private FrameworkElement CreateDetailPartView(IPartOfDetail part)
        {
            if (part is PartOfAlbum album)
            {
                return new AlbumPartView
                {
                    DataContext = new AlbumPartViewModel
                    {
                        EditorViewModel = this,
                        Detail = album,
                        Document = _document,
                        DocumentCache = _cache,
                    }
                };
            }

            if (part is PartOfPlaylist playlist)
            {
                return new PlaylistPartView
                {
                    DataContext = new PlaylistPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = playlist,
                        Document        = _document,
                        DocumentCache   = _cache,
                    }
                };
            }
            
            if (part is PartOfRel rel)
            {
                return new CharacterRelationPartView
                {
                    DataContext = new CharacterRelationPartViewModel
                    {
                        EditorViewModel = this,
                        Detail          = rel,
                        Document        = _document,
                        DocumentCache   = _cache,
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

        protected void AddDetailPart(PartOfDetail part)
        {
            part.Index = DetailParts.Count;
            DetailParts.Add(part);
            _document.Parts.Add(part);
        }
        
        private async Task AddDetailPartImpl()
        {
            var hash = DetailParts.Select(x => x.GetType())
                                  .ToHashSet();

            var iterator = CreateDetailPart().Where(x => !hash.Contains(x.GetType()));
            
            var r = await SubSystem.OptionSelection<PartOfDetail>(
                SubSystemString.SelectTitle,
                null, 
                iterator);

            if (!r.IsFinished)
            {
                return;
            }
            
            var part = r.Value;
            AddDetailPart(part);
            
            //
            //
            Successful(SubSystemString.OperationOfAddIsSuccessful);
            ResortDetailPart();
            SetDirtyState();
        }

        private void ResortDetailPart()
        {
            for (var i = 0; i < DetailParts.Count; i++)
            {
                DetailParts[i].Index = i;
            }
            ShiftDownDetailPartCommand.NotifyCanExecuteChanged();
            ShiftUpDetailPartCommand.NotifyCanExecuteChanged();
            SetDirtyState();
        }

        private async Task RemoveDetailPartImpl(PartOfDetail part)
        {
            if (part is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            DetailParts.Remove(part);
            ResortDetailPart();
            SetDirtyState();
        }
        
        private void ShiftDownDetailPartImpl(PartOfDetail module)
        {
            DetailParts.ShiftDown(module, (_, _, _) => ResortDetailPart());
        }
        
        private void ShiftUpDetailPartImpl(PartOfDetail module)
        {
            DetailParts.ShiftUp(module, (_, _, _) => ResortDetailPart());
        }


        /// <summary>
        /// 获取或设置 <see cref="DetailPart"/> 属性。
        /// </summary>
        public FrameworkElement DetailPart
        {
            get => _detailPartOfDetail;
            private set => SetValue(ref _detailPartOfDetail, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedDetailPart"/> 属性。
        /// </summary>
        public object SelectedDetailPart
        {
            get => _selectedDetailPart;
            set
            {
                SetValue(ref _selectedDetailPart, value);

                if (_selectedDetailPart is IPartOfDetail detail)
                {
                    DetailPart = CreateDetailPartView(detail);
                }
                else
                {
                    DetailPart = value as FrameworkElement;
                }
            }
        }
        
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand AddDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public RelayCommand<PartOfDetail> ShiftUpDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public RelayCommand<PartOfDetail> ShiftDownDetailPartCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)] public AsyncRelayCommand<PartOfDetail> RemoveDetailPartCommand { get; }
    }
}