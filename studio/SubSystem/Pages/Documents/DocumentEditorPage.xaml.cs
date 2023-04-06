using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{

    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(CharacterDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(ItemDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(AbilityDocumentViewModel))]
    [Connected(View = typeof(DocumentEditorPage), ViewModel = typeof(OtherDocumentViewModel))]
    public partial class DocumentEditorPage
    {
        public static readonly DependencyProperty SubViewsProperty = DependencyProperty.Register(
            nameof(SubViews),
            typeof(ObservableCollection<FrameworkElement>),
            typeof(DocumentEditorPage),
            new PropertyMetadata(default(ObservableCollection<FrameworkElement>)));
        
        public DocumentEditorPage()
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
                SubViews.Add(new CharacterInspirationView());
            }
            else if (editor is AbilityDocumentViewModel)
            {
                SubViews.Add(new AbilityBasicView());
                SubViews.Add(new CharacterInspirationView());
            }
            else if (editor is ItemDocumentViewModel)
            {
                SubViews.Add(new ItemBasicView());
                SubViews.Add(new CharacterInspirationView());
            }
            else if (editor is OtherDocumentViewModel)
            {
                SubViews.Add(new OtherBasicView());
                SubViews.Add(new CharacterInspirationView());
            }
        }


        public ObservableCollection<FrameworkElement> SubViews
        {
            get => (ObservableCollection<FrameworkElement>)GetValue(SubViewsProperty);
            set => SetValue(SubViewsProperty, value);
        }
    }
}