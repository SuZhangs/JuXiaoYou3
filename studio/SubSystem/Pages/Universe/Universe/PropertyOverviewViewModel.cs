namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class PropertyOverviewViewModel : InspectableViewModel<PropertyOverview>
    {
        public PropertyOverviewViewModel()
        {
            SpaceLayout = new ObservableCollection<SpaceConcept>();
        }
        
        public override void Save()
        {
            
        }
        
        public ObservableCollection<SpaceConcept> SpaceLayout { get; init; }
    }
}