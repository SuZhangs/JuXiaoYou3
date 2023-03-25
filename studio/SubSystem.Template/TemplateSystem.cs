using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class TemplateSystem
    {
        public static void InstallViews()
        {
            Xaml.InstallView<EditBlockView, EditBlockViewModel>();
            Xaml.InstallView<NewBlockView, NewBlockViewModel>();
            Xaml.InstallView<NewElementView, NewElementViewModel>();
            Xaml.InstallView<TemplateEditorPage, TemplateEditorViewModel>();
        }
    }
}