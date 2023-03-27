using System;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class HeaderedSubView
    {
        public void Create(object dataContext)
        {
            if (Caching)
            {
                SubView ??= (FrameworkElement)Activator.CreateInstance(Type);
            }
            else
            {
                SubView = (FrameworkElement)Activator.CreateInstance(Type);
            }

            if (SubView is null)
            {
                return;
            }

            SubView.DataContext = dataContext;
        }
        public bool Caching { get; init; }
        public string Name { get; init; }
        public Type Type { get; init; }
        public FrameworkElement SubView { get; set; }
    }
}