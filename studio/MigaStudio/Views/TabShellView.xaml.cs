using System.Windows;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    public partial class TabShellView : UserControl
    {
        public TabShellView()
        {
            InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }


        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Shell.InvalidateMeasure();
        }

        private void MouseDown_ClosePage(object sender, MouseButtonEventArgs e)
        {
        }

        public TabShell ViewModel => (TabShell)DataContext;
    }
}