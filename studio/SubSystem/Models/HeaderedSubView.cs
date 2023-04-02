﻿using System;
using System.Windows;

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
            
            SubView.DataContext = dataContext;
        }
        public string Name { get; init; }
        public Type Type { get; init; }
        public FrameworkElement SubView { get; set; }
    }
}