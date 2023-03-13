using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{

    [Connected(View = typeof(DocumentEditorView), ViewModel = typeof(CharacterDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorView), ViewModel = typeof(ItemDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorView), ViewModel = typeof(AbilityDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorView), ViewModel = typeof(OtherDocumentViewModel))]
    public partial class DocumentEditorView
    {
        public static readonly DependencyProperty SubViewsProperty = DependencyProperty.Register(
            nameof(SubViews),
            typeof(ObservableCollection<FrameworkElement>),
            typeof(DocumentEditorView),
            new PropertyMetadata(default(ObservableCollection<FrameworkElement>)));
        
        public DocumentEditorView()
        {
            InitializeComponent();
            SubViews           =  new ObservableCollection<FrameworkElement>();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = this.ViewModel<DocumentEditorVMBase>();
            
            if (vm is null)
            {
                return;
            }
            
            CreateSubViews(vm);
        }

        private void CreateSubViews(DocumentEditorVMBase editor)
        {
            if (editor is CharacterDocumentViewModel)
            {
                SubViews.Add(new CharacterBasicView());
                SubViews.Add(new DocumentDataPartView());
            }
            else if (editor is AbilityDocumentViewModel)
            {
                SubViews.Add(new AbilityBasicView());
                SubViews.Add(new DocumentDataPartView());
            }
            else if (editor is ItemDocumentViewModel)
            {
                SubViews.Add(new ItemBasicView());
                SubViews.Add(new DocumentDataPartView());
            }
            else if (editor is OtherDocumentViewModel)
            {
                SubViews.Add(new OtherBasicView());
                SubViews.Add(new DocumentDataPartView());
            }
        }


        public ObservableCollection<FrameworkElement> SubViews
        {
            get => (ObservableCollection<FrameworkElement>)GetValue(SubViewsProperty);
            set => SetValue(SubViewsProperty, value);
        }
    }
}