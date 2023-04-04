using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Controls;

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