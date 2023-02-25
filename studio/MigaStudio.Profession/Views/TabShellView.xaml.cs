using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Shell.InvalidateMeasure();
        }

        private void MouseDown_ClosePage(object sender, MouseButtonEventArgs e)
        {
        }
    }
}