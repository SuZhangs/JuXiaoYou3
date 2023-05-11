namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public interface ISpaceConceptProperty : IBrowsableElement
    {
        
    }
    
    public class SpaceConcept : Concept, ISpaceConceptProperty, IBrowsableRoot
    {
        public SpaceConcept()
        {
            InternalCollection = new ObservableCollection<IBrowsable>();
            Children           = new ReadOnlyObservableCollection<IBrowsable>(InternalCollection);
        }
        
        public void Add(ISpaceConceptProperty property)
        {
            if (property is null)
            {
                return;
            }
            
            InternalCollection.Add(property);
        }

        public void Remove(ISpaceConceptProperty property)
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
        
        protected ObservableCollection<IBrowsable> InternalCollection { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyObservableCollection<IBrowsable> Children { get; init; }
        
        
    }

    public class SpaceProperty : BrowsableProperty, ISpaceConceptProperty
    {
        
    }
}