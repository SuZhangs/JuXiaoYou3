using Acorisoft.FutureGL.MigaDB.Utils;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class PlaceholderViewModel : TabViewModel
    {
        public PlaceholderViewModel()
        {
            PageId = 
                Title = GetHashCodeIntern().ToString();
        }
        
        public override bool Removable => false;
        public override bool Uniqueness => false;
    }
}