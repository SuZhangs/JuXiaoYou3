using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Adorners;
using ColorPicker.Models;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;

namespace ViewHost.Views
{
    [Connected(View = typeof(CharacterDocumentView), ViewModel = typeof(CharacterDocumentViewModel))]
    public partial class CharacterDocumentView : UserControl
    {
        public CharacterDocumentView()
        {
            InitializeComponent();
            AdornerLayer.GetAdornerLayer(Btn)?.Add(new ThumbAdorner(Btn));
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var h = 38.7d;
            var s = 0.65d;
            var v = 1;
            
            //
            GradientStop stop = new GradientStop();

            var (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v-0.08d);
            G1.Color      = G2.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

            (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v - 0.16d);
            G3.Color  = G4.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

            (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v - 0.24d);
            G5.Color  = G6.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

            (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v - 0.32d);
            G7.Color  = G8.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

            (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v - 0.4d);
            G9.Color  = G10.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));

            (r, g, b) = ColorSpaceHelper.HsvToRgb(h, s, v - 0.48d);
            G11.Color = G12.Color = Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }
    }
}