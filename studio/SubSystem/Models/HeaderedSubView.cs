﻿using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class HeaderedSubView
    {
        public void Create(object dataContext)
        {
            
            SubView ??= (FrameworkElement)Activator.CreateInstance(Type);

            if (SubView is null)
            {
                return;
            }

            if (!ReferenceEquals(dataContext, SubView.DataContext))
            {
                SubView.DataContext = dataContext;
            }
        }
        
        public Type Type { get; init; }
        public string Name { get; init; }
        public FrameworkElement SubView { get; set; }
        
        public string DefaultColor { get; init; }
        public string HighlightColor { get; init; }
    }
}