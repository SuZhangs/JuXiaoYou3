namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public abstract class Concept : BrowsableElement
    {
        protected Concept(string uid)
        {
            Uid = uid;
        }
        
        protected Concept(){}
    }
}