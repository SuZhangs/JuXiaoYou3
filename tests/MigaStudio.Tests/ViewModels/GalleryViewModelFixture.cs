using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace MigaStudio.Tests.ViewModels
{
    [TestClass, TestCategory("ViewModels")]
    public class GalleryViewModelFixture
    {
        [TestMethod]
        public void FirstStart_PageIndexShouldBeOne()
        {
            var gallery = new DocumentGalleryViewModel();
            Assert.AreEqual(gallery.PageIndex, 1);
        }
    }
}