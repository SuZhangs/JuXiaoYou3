using System.IO;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Pages.Composes;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public static class SubSystem
    {
        public static Task<Op<DocumentCache>> NewDocument()
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, NewDocumentViewModel>();
        }

        public static void ImageView(string fileName)
        {
            var vm = new ImageViewModel
            {
                Source = new BitmapImage(new Uri(fileName, UriKind.Absolute))
            };
            
            new ImageView
            {
                DataContext = vm
            }.Show();
        }

        /// <summary>
        /// 选择选项视图
        /// </summary>
        /// <param name="title">视图的标题</param>
        /// <param name="selected">选择的选项</param>
        /// <param name="options">所有选项</param>
        /// <typeparam name="TOption">选项类型</typeparam>
        /// <returns>返回一个操作结果</returns>
        public static Task<Op<TOption>> Selection<TOption>(string title, object selected, IEnumerable<object> options)
        {
            var optionVM = Xaml.GetViewModel<OptionSelectionViewModel>();
            var parameter = new Parameter
            {
                Args = new[]
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
            //
            // Commons
            Xaml.InstallView<ImageEditView, ImageEditViewModel>();
            Xaml.InstallView<MusicPlayerView, MusicPlayerViewModel>();
            Xaml.InstallView<OptionSelectionView, OptionSelectionViewModel>();
            Xaml.InstallView<DocumentPickerView, DocumentPickerViewModel>();
            
            //
            // Compose
            Xaml.InstallView<ComposeEditorPage, ComposeEditorViewModel>();

            //
            // Commons
            Xaml.InstallView<NewDocumentView, NewDocumentViewModel>();
            Xaml.InstallView<DocumentGalleryExPage, DocumentGalleryViewModelEx>();

            //
            // Document
            Xaml.InstallView<DetailPartSelectorView, DetailPartSelectorViewModel>();
            Xaml.InstallView<ManageSurveyView, ManageSurveyViewModel>();
            Xaml.InstallView<NewPresentationView, NewPresentationViewModel>();
            Xaml.InstallView<NewSurveyView, NewSurveyViewModel>();
            Xaml.InstallView<EditPresentationView, EditPresentationViewModel>();
            Xaml.InstallView<EditStringPresentationView, EditStringPresentationViewModel>();
            Xaml.InstallView<EditChartPresentationView, EditChartPresentationViewModel>();
            Xaml.InstallView<EditPlainTextView, EditPlainTextViewModel>();
            Xaml.InstallView<ModuleSelectorView, ModuleSelectorViewModel>();
            Xaml.InstallView<DocumentEditorPage, AbilityDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, CharacterDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, ItemDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, OtherDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, GeographyDocumentViewModel>();

            //
            // Relationships
            Xaml.InstallView<NewRelativeView, NewRelativeViewModel>();
            Xaml.InstallView<NewRelativePresetView, NewRelativePresetViewModel>();
            Xaml.InstallView<RelativePresetPage, RelativePresetViewModel>();
            Xaml.InstallView<CharacterPedigreePage, CharacterPedigreeViewModel>();
            Xaml.InstallView<CharacterRelationshipPage, CharacterRelationshipViewModel>();

            //
            // Template
            Xaml.InstallView<EditBlockView, EditBlockViewModel>();
            Xaml.InstallView<NewBlockView, NewBlockViewModel>();
            Xaml.InstallView<NewElementView, NewElementViewModel>();
            Xaml.InstallView<TemplateEditorPage, TemplateEditorViewModel>();
            Xaml.InstallView<TemplateGalleryPage, TemplateGalleryViewModel>();
            Xaml.InstallView<ModuleManifestView, ModuleManifestViewModel>();

            //
            // Template
            Xaml.InstallView<WeaponEditorPage, WeaponEditorViewModel>();
            
            ViewMapper.Initialize();
        }
    }
}