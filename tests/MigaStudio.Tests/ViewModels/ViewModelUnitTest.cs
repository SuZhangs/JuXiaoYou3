using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor;

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

        [TestMethod]
        public void TemplateEditorFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new TemplateEditorViewModel());
        }

        [TestMethod]
        public void DocumentGalleryFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new DocumentGalleryViewModel());
        }
        
        [TestMethod]
        public void CharacterDocumentFixture()
        {
            ViewModelUnitTestArchitecture.UnitTest(new CharacterDocumentViewModel());
        }
    }
}