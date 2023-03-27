using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace MigaStudio.Tests.ViewModels
{
    [TestClass, TestCategory("ViewModels")]
    public class GalleryViewModelFixture
    {
        
        /// <summary>
        /// 测试新建视图模型是否会出现异常
        /// </summary>
        [TestMethod]
        public void ConstructorUnitTest()
        {
            var vm = new DocumentGalleryViewModel();
            
            ViewModelUnitTest.AssertAllPropertyWasNotNull(vm);
        }
        
        [TestMethod]
        public void FirstStart_PageIndexShouldBeOne()
        {
            var gallery = new DocumentGalleryViewModel();
            Assert.AreEqual(gallery.PageIndex, 1);
        }
    }
}