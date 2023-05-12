namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class SpaceConceptOverview : Overview
    {
        public SpaceConceptOverview() : base("text.Universe.Overview.Space")
        {
        }
        
        public void Add(SpaceConcept property)
        {
            if (property is null)
            {
                return;
            }
            
            InternalCollection.Add(property);
        }

        public void Remove(SpaceConcept property)
        {
            if (property is null)
            {
                return;
            }
            
            InternalCollection.Remove(property);
        }

        public void Clear()
        {
            InternalCollection.Clear();
        }
    }
}