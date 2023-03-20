using System.Windows;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    public partial class TabShellView:ForestUserControl 
    {
        public TabShellView()
        {
            InitializeComponent();
        }

        private void MouseDown_ClosePage(object sender, MouseButtonEventArgs e)
        {
        }

        public TabShell ViewModel => (TabShell)DataContext;
    }
}