using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class UniverseEditorViewModelProxy : BindingProxy<UniverseEditorBase>
    {
    }

    public abstract class UniverseEditorBase : TabViewModel
    {
        public override bool Removable => false;
    }
}