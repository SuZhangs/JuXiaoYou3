namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public abstract class InspectableViewModel : ViewModelBase
    {
        public abstract void Save();
        public UniverseEditorViewModel Owner { get; init; }
    }

    public abstract class InspectableViewModel<TBrowsable> : InspectableViewModel where TBrowsable : IBrowsable
    {
        public TBrowsable Browsable { get; init; }
    }
}