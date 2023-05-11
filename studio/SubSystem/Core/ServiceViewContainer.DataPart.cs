﻿using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using DocumentEditorViewModel = Acorisoft.FutureGL.MigaStudio.Pages.Commons.DocumentEditorBase;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    using DataPartDictionary = System.Collections.Generic.Dictionary<System.Type, Acorisoft.FutureGL.MigaStudio.Core.ViewFactory<DocumentEditorViewModel>>;
    
    partial class ServiceViewContainer
    {
        private static readonly DataPartDictionary Mappings = new DataPartDictionary();

        public static void Add<T>(ViewFactory<DocumentEditorBase> expression)
        {
            if (expression is null)
            {
                return;
            }

            Mappings.TryAdd(typeof(T), expression);
        }

        private static UserControl GetAlbumView(DocumentEditorBase owner, object instance)
        {
            return new AlbumPartView
            {
                DataContext = new AlbumPartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfAlbum)instance,
                }
            };
        }

        private static UserControl GetPlaylistView(DocumentEditorBase owner, object instance)
        {
            return new PlaylistPartView
            {
                DataContext = new PlaylistPartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfPlaylist)instance,
                }
            };
        }

        private static UserControl GetCharacterRel(DocumentEditorBase owner, object instance)
        {
            var d = (DocumentEditorVMBase)owner;
            return new CharacterRelshipPartView
            {
                DataContext = new CharacterRelPartViewModel(d, (PartOfRel)instance)
            };
        }

        private static UserControl GetSurvey(DocumentEditorBase owner, object instance)
        {
            return new SurveyPartView
            {
                DataContext = new SurveyPartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfSurvey)instance,
                }
            };
        }

        private static UserControl GetStickyNote(DocumentEditorBase owner, object instance)
        {
            return new StickyNotePartView
            {
                DataContext = new StickyNotePartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfStickyNote)instance,
                }
            };
        }

        private static UserControl GetSentenceView(DocumentEditorBase owner, object instance)
        {
            return new SentencePartView
            {
                DataContext = new SentencePartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfSentence)instance,
                }
            };
        }

        private static UserControl GetPrototypeView(DocumentEditorBase owner, object instance)
        {
            return new PrototypePartView
            {
                DataContext = new PrototypePartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfPrototype)instance
                }
            };
        }

        private static UserControl GetAppraiseView(DocumentEditorBase owner, object instance)
        {
            return new AppraisePartView
            {
                DataContext = new AppraisePartViewModel
                {
                    Owner  = owner,
                    Detail = (PartOfAppraise)instance
                }
            };
        }

        private static UserControl GetDetailSetting(DocumentEditorBase owner, object instance)
        {
            return new DetailPartSettingView
            {
                DataContext = owner
            };
        }

        public static UserControl GetView(DocumentEditorBase owner, object viewModel)
        {
            if (viewModel is null)
            {
                return null;
            }

            if (Mappings.TryGetValue(viewModel.GetType(), out var mapping))
            {
                return mapping(owner, viewModel);
            }

            return new UserControl
            {
                DataContext = viewModel
            };
        }
    }
}