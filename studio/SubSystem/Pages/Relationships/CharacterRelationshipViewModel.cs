﻿using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class CharacterRelationshipViewModel : TabViewModel
    {
        private DocumentCache _selectedDocument;
        private Visibility    _relationshipPaneVisibility;
        private int           _version;

        public CharacterRelationshipViewModel()
        {
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager            = dbMgr;
            DocumentEngine             = dbMgr.GetEngine<DocumentEngine>();
            Graph                      = new CharacterGraph();
            Relationships              = new ObservableCollection<CharacterRelationship>();
            RelationshipPaneVisibility = Visibility.Collapsed;
            AddDocumentCommand         = AsyncCommand(NewDocumentImpl);
            OpenDocumentCommand        = Command<DocumentCache>(OpenDocumentImpl, HasItem, true);
            ResetDocumentCommand       = Command(Reset, () => HasItem(SelectedDocument), true);
            AddRelCommand              = AsyncCommand<DocumentCache>(AddRelationshipImpl, HasItem, true);
            _version                   = DocumentEngine.Version;
            Initialize();
        }

        private void Initialize()
        {
            Graph.Clear();
            Graph.AddVertexRange(DocumentEngine.DocumentCacheDB
                                               .FindAll());
            Graph.AddEdgeRange(DocumentEngine.GetRelationships());
        }

        #region OnStart / OnResume

        public sealed override void OnStart()
        {
            Initialize();
            base.OnStart();
        }

        public sealed override void Resume()
        {
            if (_version != DocumentEngine.Version)
            {
                Initialize();
                _version = DocumentEngine.Version;
            }
        }

        #endregion

        #region Document: Add / Remove / Reset
        

        private async Task NewDocumentImpl()
        {
            await DocumentUtilities.AddDocument(DocumentEngine, DocumentType.Character, x =>
            {
                Graph.AddVertex(x);
            });
        }
        
        private void OpenDocumentImpl(DocumentCache cache)
        {
            DocumentUtilities.OpenDocument(Controller, cache);
        }

        private void Reset()
        {
            SelectedDocument = null;
        }
        
        #endregion

        #region Relationship: Add / Remove

        private async Task AddRelationshipImpl(DocumentCache source)
        {
            var hash = Relationships.Select(x => x.Id)
                                    .ToHashSet();
            var r = await DocumentPickerViewModel.Select(DocumentEngine.GetDocuments(DocumentType.Character)
                                                                       .Where(x => !hash.Contains(x.Id)));

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await NewRelationshipViewModel.New(source, r.Value);
            
            if (!r1.IsFinished)
            {
                return;
            }

            DocumentEngine.AddRelationship(r1.Value);
            Graph.AddEdge(r1.Value);
        }

        private async Task RemoveRelationshipImpl(CharacterRelationship rel)
        {
            if (rel is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }
            
            
        }
        #endregion

        /// <summary>
        /// 人物关系面板的可视性
        /// </summary>
        public Visibility RelationshipPaneVisibility
        {
            get => _relationshipPaneVisibility;
            set => SetValue(ref _relationshipPaneVisibility, value);
        }

        /// <summary>
        /// 选择的文档
        /// </summary>
        public DocumentCache SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                SetValue(ref _selectedDocument, value);
                RelationshipPaneVisibility = value is not null ? Visibility.Visible : Visibility.Collapsed;
                Relationships.Clear();
                if (_selectedDocument is null)
                {
                    return;
                }

                Graph.TryGetInEdges(value, out var edgeA);
                Graph.TryGetOutEdges(value, out var edgeB);

                var total = edgeA.Concat(edgeB);
                Relationships.AddRange(total);
            }
        }

        /// <summary>
        /// 人物关系图
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public CharacterGraph Graph { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public DocumentEngine DocumentEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        /// <summary>
        /// 当前的所有人物关系
        /// </summary>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<CharacterRelationship> Relationships { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<DocumentCache> OpenDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand ResetDocumentCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<DocumentCache> AddRelCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> RemoveRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand CaptureCommand { get; }
    }
}