using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(QuickStartView), ViewModel = typeof(QuickStartController))]
    public partial class QuickStartView:ForestUserControl 
    {
        public QuickStartView()
        {
            InitializeComponent();
        }

    }
}