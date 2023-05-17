namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class PropertyOverviewViewModel : InspectableViewModel<PropertyOverview>
    {
        public PropertyOverviewViewModel()
        {
            SpaceLayout = new ObservableCollection<SpaceConcept>();
        }

        public ObservableCollection<SpaceConcept> SpaceLayout { get; init; }
    }
}