using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;

namespace MigaStudio.Tests.ViewModels
{
    [TestClass, TestCategory("ViewModels")]
    public class ViewModelUnitTest
    {
        
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //
            // 初始化
            ViewModelUnitTestArchitecture.Initialize(Xaml.Container);
        }

        #region DocumentViewModels

        
        
        [TestMethod]
        public void CharacterDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new CharacterDocumentViewModel());
        }



        [TestMethod]
        public void ItemDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ItemDocumentViewModel());
        }
        
        
        [TestMethod]
        public void AbilityDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new AbilityDocumentViewModel());
        }
        
        
        [TestMethod]
        public void GeographyDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new GeographyDocumentViewModel());
        }
        
        
        [TestMethod]
        public void OtherDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new OtherDocumentViewModel());
        }

        #endregion
        
        
        #region TemplateViewModels

        
        [TestMethod]
        public void TemplateEditorFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new TemplateEditorViewModel());
        }

        
        [TestMethod]
        public void TemplateGalleryFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new TemplateGalleryViewModel());
        }
        
        
        
        
        
        [TestMethod]
        public void ModuleManifestFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ModuleManifestViewModel());
        }
        
        
        [TestMethod]
        public void ModuleSelectorFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ModuleSelectorViewModel());
        }
        
        
        [TestMethod]
        public void EditBlockFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new EditBlockViewModel());
        }
        
        
        [TestMethod]
        public void NewElementFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new NewElementViewModel());
        }

        
        [TestMethod]
        public void NewBlockFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new NewBlockViewModel());
        }
        
        #endregion

        [TestMethod]
        public void DocumentGalleryFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new DocumentGalleryViewModelEx());
        }
        
        [Obsolete("不适合自动化测试，改为人工测试")]
        public void SettingFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new SettingViewModel());
        }
        
        
        [TestMethod]
        public void HomeFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new HomeViewModel());
        }
    }
}