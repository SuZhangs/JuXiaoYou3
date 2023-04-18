﻿using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterRelPartViewModelProxy : BindingProxy<CharacterRelPartViewModel>{}
    
    public class CharacterRelPartViewModel : ViewModelBase
    {
        private DocumentCache _selectedDocument;
        private Visibility    _relationshipPaneVisibility;
        private int           _version;

        public CharacterRelPartViewModel(DocumentEditorBase owner, PartOfRel rel)
        {
            Owner  = owner;
            Detail = rel;
            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager            = dbMgr;
            DocumentEngine             = dbMgr.GetEngine<DocumentEngine>();
            Graph                      = new CharacterGraph();
            Relationships              = new ObservableCollection<CharacterRelationship>();
            RelationshipPaneVisibility = Visibility.Collapsed;
            AddRelCommand              = AsyncCommand<DocumentCache>(AddRelationshipImpl);
            CaptureCommand             = AsyncCommand<FrameworkElement>(CaptureImpl, HasItem);
            RemoveRelCommand           = AsyncCommand<CharacterRelationship>(RemoveRelationshipImpl, HasItem, true);
            _version                   = DocumentEngine.Version;
        }

        private void Initialize()
        {
            var cache = Owner.Cache;
            var rels = DocumentEngine.GetRelationships(DocumentType.Character, cache.Id)
                                     .ToArray();
            var hash = rels.Flat(x => new[] { x.Target.Id, x.Source.Id })
                           .ToHashSet();
            Graph.Clear();
            Graph.AddVertexRange(DocumentEngine.DocumentCacheDB
                                               .Find(x => hash.Contains(x.Id)));
            Graph.AddEdgeRange(rels);
            SelectedDocument = cache;
        }

        #region OnStart / OnResume

        public sealed override void Start()
        {
            Initialize();
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


        #region Relationship: Add / Remove

        private async Task AddRelationshipImpl(DocumentCache source)
        {
            var hash = Relationships.Flat(x => new []{ x.Source.Id, x.Target.Id})
                                    .ToHashSet();
            hash.Add(source.Id);
            var r = await DocumentPickerViewModel.Select(DocumentEngine.GetDocuments(DocumentType.Character)
                                                                       .Where(x => !hash.Contains(x.Id)));

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await NewRelationshipViewModel.New(source, r.Value, DocumentType.Character);

            if (!r1.IsFinished)
            {
                return;
            }

            DocumentEngine.AddRelationship(r1.Value);
            Graph.AddEdge(r1.Value);

            if (SelectedDocument.Id == source.Id)
            {
                Relationships.Add(r1.Value);
            }
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

            DocumentEngine.RemoveRelationship(rel.Id);
            Relationships.Remove(rel);
            Graph.RemoveEdge(rel);
        }

        #endregion

        private async Task CaptureImpl(FrameworkElement element)
        {
            if (element is null)
            {
                return;
            }

            var savedlg = FileIO.Save(SubSystemString.PngFilter, "*.png");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            var ms = Xaml.CaptureToStream(element);
            await System.IO.File.WriteAllBytesAsync(savedlg.FileName, ms.GetBuffer());
        }

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
                Relationships.AddRange(edgeA);
                Relationships.AddRange(edgeB);
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
        public AsyncRelayCommand<DocumentCache> AddRelCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> EditRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<CharacterRelationship> RemoveRelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<FrameworkElement> CaptureCommand { get; }

        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorBase Owner { get;}

        /// <summary>
        /// 
        /// </summary>
        public PartOfRel Detail { get; }

        private bool _isHidden;
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                SetValue(ref _isHidden, value);
                _relationshipPaneVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                RaiseUpdated(nameof(RelationshipPaneVisibility));
            }
        }
    }
}