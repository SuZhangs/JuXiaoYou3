namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class UniversalIntroduction : Introduction
    {
        public UniversalIntroduction() : base("text.Universe.Overview")
        {
        }
        
        public void Add(Concept property)
        {
            if (property is null)
            {
                return;
            }
            
            InternalCollection.Add(property);
        }

        public void Remove(Concept property)
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