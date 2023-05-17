namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public abstract class InspectableViewModel : ViewModelBase
    {
        public UniverseEditorViewModel Owner { get; init; }
    }

    public abstract class InspectableViewModel<TBrowsable> : IsolatedViewModel<UniverseEditorViewModel, TBrowsable> where TBrowsable : class, IBrowsable
    {
        public TBrowsable Browsable { get; init; }
    }
}