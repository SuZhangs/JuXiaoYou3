using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor;

namespace MigaStudio.Tests.ViewModels
{
    [TestClass, TestCategory("ViewModels")]
    public class TemplateEditorVMFixture
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //
            // 初始化
            ViewModelUniTest.Initialize(Xaml.Container);
        }
        
        /// <summary>
        /// 测试新建视图模型是否会出现异常
        /// </summary>
        [TestMethod]
        public void ConstructorUnitTest()
        {
            var vm = new TemplateEditorViewModel();
            
            ViewModelUniTest.AssertAllClassPropertyWasNotNull(vm);
        }
    }
}