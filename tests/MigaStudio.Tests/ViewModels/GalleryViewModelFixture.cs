using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace MigaStudio.Tests.ViewModels
{
    public class GalleryViewModelFixture
    {
        
        /// <summary>
        /// 测试新建视图模型是否会出现异常
        /// </summary>
        public void ConstructorUnitTest()
        {
            var vm = new DocumentGalleryViewModelEx();
            
            Core.ViewModelUnitTestArchitecture.AssertAllPropertyWasNotNull(vm);
        }
        
        public void FirstStart_PageIndexShouldBeOne()
        {
            var gallery = new DocumentGalleryViewModelEx();
            Assert.AreEqual(gallery.PageIndex, 1);
        }
    }
}

namespace MigaStudio.Tests.ViewModels
{
    public class GalleryViewModelFixture2
    {

        /// <summary>
        /// 测试新建视图模型是否会出现异常
        /// </summary>
        public void ConstructorUnitTest()
        {
            var vm = new DocumentGalleryViewModelEx();

            Core.ViewModelUnitTestArchitecture.AssertAllPropertyWasNotNull(vm);
        }

        public void FirstStart_PageIndexShouldBeOne()
        {
            var gallery = new DocumentGalleryViewModelEx();
            Assert.AreEqual(gallery.PageIndex, 1);
        }
    }
}