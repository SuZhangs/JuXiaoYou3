using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class RelationshipGeometry : Control
    {
        static RelationshipGeometry()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RelationshipGeometry), 
                new FrameworkPropertyMetadata(typeof(RelationshipGeometry)));
        }


        public static readonly DependencyProperty TopNameProperty = DependencyProperty.Register(
            nameof(TopName),
            typeof(string),
            typeof(RelationshipGeometry),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty BottomNameProperty = DependencyProperty.Register(
            nameof(BottomName),
            typeof(string),
            typeof(RelationshipGeometry),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UnifiedNameProperty = DependencyProperty.Register(
            nameof(UnifiedName),
            typeof(string),
            typeof(RelationshipGeometry),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty RelationshipProperty = DependencyProperty.Register(
            nameof(Relationship),
            typeof(Relationship),
            typeof(RelationshipGeometry),
            new PropertyMetadata(default(Relationship)));


        public static readonly DependencyProperty IsBidirectionProperty = DependencyProperty.Register(
            nameof(IsBidirection),
            typeof(bool),
            typeof(RelationshipGeometry),
            new PropertyMetadata(Boxing.True));

        public bool IsBidirection
        {
            get => (bool)GetValue(IsBidirectionProperty);
            set => SetValue(IsBidirectionProperty, Boxing.Box(value));
        }

        public Relationship Relationship
        {
            get => (Relationship)GetValue(RelationshipProperty);
            set => SetValue(RelationshipProperty, value);
        }

        public string UnifiedName
        {
            get => (string)GetValue(UnifiedNameProperty);
            set => SetValue(UnifiedNameProperty, value);
        }
        
        public string BottomName
        {
            get => (string)GetValue(BottomNameProperty);
            set => SetValue(BottomNameProperty, value);
        }
        public string TopName
        {
            get => (string)GetValue(TopNameProperty);
            set => SetValue(TopNameProperty, value);
        }
    }
}