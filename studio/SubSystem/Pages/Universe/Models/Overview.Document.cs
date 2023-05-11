namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class PropertyOverview : Overview
    {
        public PropertyOverview(string uid) : base(uid)
        {
            
        }
        
        public void Add(BrowsableProperty property)
        {
            if (property is null)
            {
                return;
            }
            
            InternalCollection.Add(property);
        }

        public void Remove(BrowsableProperty property)
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