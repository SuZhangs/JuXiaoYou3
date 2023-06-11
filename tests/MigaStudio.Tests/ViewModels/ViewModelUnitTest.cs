using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting.STAExtensions;

namespace MigaStudio.Tests.ViewModels
{
    [STATestClass]
    [TestCategory("ViewModels")]
    public class ViewModelUnitTest
    {
        [ClassInitialize]
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //
            // 初始化
            ViewModelUnitTestArchitecture.Initialize(Xaml.Container);
        }

        #region DocumentViewModels

        
        
        [STAThread]
        [TestMethod]
        public void CharacterDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new CharacterDocumentViewModel());
        }



        [STAThread]
        [TestMethod]
        public void ItemDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ItemDocumentViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void SkillDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new SkillDocumentViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void GeographyDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new GeographyDocumentViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void OtherDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new OtherDocumentViewModel());
        }

        #endregion
        
        
        #region TemplateViewModels

        
        [STAThread]
        [TestMethod]
        public void TemplateEditorFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new TemplateEditorViewModel());
        }

        
        [STAThread]
        [TestMethod]
        public void TemplateGalleryFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new TemplateGalleryViewModel());
        }
        
        
        
        
        
        [STAThread]
        [TestMethod]
        public void ModuleManifestFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ModuleManifestViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void ModuleSelectorFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new ModuleSelectorViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void EditBlockFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new EditBlockViewModel());
        }
        
        
        [STAThread]
        [TestMethod]
        public void NewElementFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new NewElementViewModel());
        }

        
        [STAThread]
        [TestMethod]
        public void NewBlockFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new NewBlockViewModel());
        }
        
        #endregion

        [STAThread]
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