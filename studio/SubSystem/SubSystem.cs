using System;
using System.IO;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public static class SubSystem
    {
        public static Task<Op<DocumentCache>> NewDocumentWizard()
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, NewDocumentWizardViewModel>();
        }
        
        public static async Task ImageView(string fileName)
        {
            await Xaml.Get<IDialogService>()
                       .Dialog(new ImageViewModel(),new RouteEventArgs
                       {
                           Args = new []
                           {
                               fileName
                           }
                       });
        }

        /// <summary>
        /// 选择选项视图
        /// </summary>
        /// <param name="title">视图的标题</param>
        /// <param name="selected">选择的选项</param>
        /// <param name="options">所有选项</param>
        /// <typeparam name="TOption">选项类型</typeparam>
        /// <returns>返回一个操作结果</returns>
        public static Task<Op<TOption>> OptionSelection<TOption>(string title, object selected, IEnumerable<object> options)
        {
            var optionVM = Xaml.GetViewModel<OptionSelectionViewModel>();
            var parameter = new RouteEventArgs
            {
                Args = new object[]
                {
                    selected,
                    options
                }
            };

            optionVM.Title = title;
            return Xaml.Get<IDialogService>()
                       .Dialog<TOption>(optionVM, parameter);
        }

        public static void InstallLanguages()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "SubSystem.en.ini",
                CultureArea.French   => "SubSystem.fr.ini",
                CultureArea.Japanese => "SubSystem.jp.ini",
                CultureArea.Korean   => "SubSystem.kr.ini",
                CultureArea.Russian  => "SubSystem.ru.ini",
                _                    => "SubSystem.cn.ini",
            };

            Language.AppendLanguageSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", fileName));
        }

        public static void InstallViews()
        {
            Xaml.InstallView<ImageEditView, ImageEditViewModel>();
            Xaml.InstallView<ImageView, ImageViewModel>();
            Xaml.InstallView<MusicPlayerView, MusicPlayerViewModel>();
            Xaml.InstallView<OptionSelectionView, OptionSelectionViewModel>();

            Xaml.InstallView<NewDocumentWizard, NewDocumentWizardViewModel>();
            Xaml.InstallView<DocumentGalleryPage, DocumentGalleryViewModel>();

            Xaml.InstallView<DetailPartSelectorView, DetailPartSelectorViewModel>();
            Xaml.InstallView<NewPreviewBlockView, NewPreviewBlockViewModel>();
            Xaml.InstallView<EditPreviewBlockView, EditPreviewBlockViewModel>();
            Xaml.InstallView<EditChartPreviewBlockView, EditChartPreviewBlockViewModel>();
            Xaml.InstallView<ModuleSelectorView, ModuleSelectorViewModel>();
            Xaml.InstallView<DocumentEditorPage, AbilityDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, CharacterDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, ItemDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, OtherDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, GeographyDocumentViewModel>();

            Xaml.InstallView<CharacterRelationshipPage, CharacterRelationshipViewModel>();

            Xaml.InstallView<EditBlockView, EditBlockViewModel>();
            Xaml.InstallView<NewBlockView, NewBlockViewModel>();
            Xaml.InstallView<NewElementView, NewElementViewModel>();
            Xaml.InstallView<TemplateEditorPage, TemplateEditorViewModel>();
            Xaml.InstallView<TemplateGalleryPage, TemplateGalleryViewModel>();
            Xaml.InstallView<ModuleManifestView, ModuleManifestViewModel>();
        }
    }
}