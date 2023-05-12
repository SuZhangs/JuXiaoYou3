using Acorisoft.FutureGL.MigaStudio.Core;

namespace Acorisoft.FutureGL.MigaStudio
{
    partial class App
    {
        private static void InstallInMemoryService(IInMemoryServiceHost attachable)
        {
            attachable.Add(new ColorService());
        } 
    }
}