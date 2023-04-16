using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class CharacterRelationshipViewModel : TabViewModel
    {
        private DocumentCache _selectedDocument;
        private Visibility    _relationshipPaneVisibility;

        public CharacterRelationshipViewModel()
        {
            Graph                      = new CharacterGraph();
            Relationships              = new ObservableCollection<CharacterRelationship>();
            RelationshipPaneVisibility = Visibility.Collapsed;
            Initialize();
        }

        public void Initialize()
        {
            var c1 =new DocumentCache
            {
                Id     = "1",
                Name   = "角色1",
                Avatar = @"E:\企划\橘小柚\社区\美术\罗易斯_1.png",
            };
            var c2 =new DocumentCache
            {
                Id     = "2",
                Name   = "角色2",
                Avatar = @"E:\企划\橘小柚\社区\美术\罗易斯_2.png",
            };
            var c3 =new DocumentCache
            {
                Id     = "3",
                Name   = "角色3",
                Avatar = @"E:\企划\橘小柚\社区\美术\罗易斯_3.png",
            };
            var c4 =new DocumentCache
            {
                Id     = "4",
                Name   = "角色4",
                Avatar = @"E:\企划\橘小柚\社区\美术\罗易斯_4.png",
            };
            var c5 = new DocumentCache
            {
                Id     = "5",
                Name   = "角色5",
                Avatar = @"E:\企划\橘小柚\社区\美术\罗易斯_5.png",
            };

            Graph.AddVertex(c1);
            Graph.AddVertex(c2);
            Graph.AddVertex(c3);
            Graph.AddVertex(c4);
            Graph.AddVertex(c5);
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c1,
                Target = c2,
                Name   = "人物关系1"
            });
            
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c1,
                Target = c2,
                Name   = "人物关系1"
            });
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c2,
                Target = c3,
                Name   = "人物关系1"
            });
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c3,
                Target = c4,
                Name   = "人物关系1"
            });
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c4,
                Target = c5,
                Name   = "人物关系1"
            });
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c1,
                Target = c5,
                Name   = "人物关系1"
            });
            Graph.AddEdge(new CharacterRelationship
            {
                Id     = ID.Get(),
                Source = c2,
                Target = c4,
                Name   = "人物关系1"
            });
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
                if (_selectedDocument is null)
                {
                    return;
                }

                Graph.TryGetInEdges(value, out var edgeA);
                Graph.TryGetOutEdges(value, out var edgeB);

                var total = edgeA.Concat(edgeB);
                Relationships.AddRange(total, true);
            }
        }

        /// <summary>
        /// 人物关系图
        /// </summary>
        public CharacterGraph Graph { get; }

        /// <summary>
        /// 当前的所有人物关系
        /// </summary>
        public ObservableCollection<CharacterRelationship> Relationships { get; }

        public AsyncRelayCommand AddDocumentCommand { get; }
        public AsyncRelayCommand RemoveDocumentCommand { get; }
        public AsyncRelayCommand OpenDocumentCommand { get; }
        public AsyncRelayCommand ResetDocumentCommand { get; }
        public AsyncRelayCommand AddRelCommand { get; }
        public AsyncRelayCommand RemoveRelCommand { get; }
        public AsyncRelayCommand RelayoutCommand { get; }
        public AsyncRelayCommand CaptureCommand { get; }
        public AsyncRelayCommand SwitchModeCommand { get; }
    }
}