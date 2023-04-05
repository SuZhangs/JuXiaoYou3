using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public abstract class HierarchicalViewNode : SubViewBase
    {
        
    }

    public sealed class HierarchicalSubView : HierarchicalViewNode
    {
        public void Create(object dataContext)
        {
            
            SubView ??= (FrameworkElement)Activator.CreateInstance(Type);

            if (SubView is null)
            {
                return;
            }
            
            SubView.DataContext = dataContext;
        }
        public Type Type { get; init; }
        public FrameworkElement SubView { get; set; }
    }

    public sealed class HierarchicalViewFolder : HierarchicalViewNode
    {
        
    }
}