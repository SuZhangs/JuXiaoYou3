using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using DocumentEditorViewModel = Acorisoft.FutureGL.MigaStudio.Pages.Commons.DocumentEditorBase;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    using DataPartDictionary = System.Collections.Generic.Dictionary<System.Type, Acorisoft.FutureGL.MigaStudio.Core.ViewFactory<DocumentEditorViewModel>>;

    partial class ServiceViewContainer
    {
        private static FrameworkElement GetCharacterRel(object owner, object instance)
        {
            var d = (DocumentEditorVMBase)owner;
            return new CharacterRelshipPartView
            {
                DataContext = new CharacterRelPartViewModel(d, (PartOfRel)instance)
            };
        }
        
        private static void UseDataPart<TPart, TViewModel, TView>()
            where TPart : DataPart
            where TViewModel : IsolatedViewModel<DocumentEditorBase, TPart>, new()
            where TView : FrameworkElement, new()
        {
            _service.Isolate<DocumentEditorBase, TPart, TView, TViewModel>();
        }

        private static void UseDetailSetting()
        {
            _service.Direct<DocumentEditorBase, DetailPartSettingPlaceHolder, DetailPartSettingView>();
        }

        public static FrameworkElement GetView(DocumentEditorBase owner, object viewModel) => _service.GetView(owner, viewModel);
    }
}