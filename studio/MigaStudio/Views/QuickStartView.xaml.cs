using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(QuickStartView), ViewModel = typeof(QuickStartController))]
    public partial class QuickStartView : UserControl
    {
        public QuickStartView()
        {
            InitializeComponent();

            SizeChanged += (_, _) =>
            {
                Panel. TileCount = (int)((ActualWidth - (Margin.Left + Margin.Right)) / Panel.TileSize);
            };
        }

    }
}