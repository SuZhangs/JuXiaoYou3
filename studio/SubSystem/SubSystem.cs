using System;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using ImageEditViewModel = Acorisoft.FutureGL.MigaStudio.Pages.Commons.ImageEditViewModel;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public static class SubSystem
    {
        public static Task<Op<DocumentCache>> NewDocumentWizard()
        {
            return Xaml.Get<IDialogService>().Dialog<DocumentCache, NewDocumentWizardViewModel>();
        }
        
        /// <summary>
        /// 选择选项视图
        /// </summary>
        /// <param name="title">视图的标题</param>
        /// <param name="selected">选择的选项</param>
        /// <param name="options">所有选项</param>
        /// <typeparam name="TOption">选项类型</typeparam>
        /// <returns>返回一个操作结果</returns>
        public static Task<Op<TOption>> OptionSelection<TOption>(string title, object selected, object[] options)
        {
            var optionVM = Xaml.GetViewModel<OptionSelectionViewModel>();
            var parameter = new Parameter
            {
                Args = new object[]
                {
                    selected,
                    options
                }
            };
            
            optionVM.Title = title;
            return Xaml.Get<IDialogService>().Dialog<TOption>(optionVM, parameter);
        }

        public static void InstallViews()
        {
            Xaml.InstallView<ImageEditView, ImageEditViewModel>();
            Xaml.InstallView<OptionSelectionView, OptionSelectionViewModel>();

            Xaml.InstallView<NewDocumentWizard, NewDocumentWizardViewModel>();
            Xaml.InstallView<DocumentGallery, DocumentGalleryViewModel>();

            Xaml.InstallView<DocumentEditorView, AbilityDocumentViewModel>();
            Xaml.InstallView<DocumentEditorView, CharacterDocumentViewModel>();
            Xaml.InstallView<DocumentEditorView, ItemDocumentViewModel>();
            Xaml.InstallView<DocumentEditorView, OtherDocumentViewModel>();
            Xaml.InstallView<GeographyEditorView, GeographyDocumentViewModel>();
        }
    }
}