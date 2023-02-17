using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Styles
{
    public class ForestDarkTheme : ForestThemeSystem
    {
        public ForestDarkTheme()
        {
            InitializeColors();
        }
        
        private void InitializeColors()
        {
            Colors.TryAdd((int)ForestTheme.HighlightA1, Color.FromRgb(0xff, 0xda, 0x96));
            Colors.TryAdd((int)ForestTheme.HighlightA2, Color.FromRgb(0xff, 0xc2, 0x54));
            Colors.TryAdd((int)ForestTheme.HighlightA3, Color.FromRgb(0xff, 0xab, 0x12));
            Colors.TryAdd((int)ForestTheme.HighlightA4, Color.FromRgb(0xd4, 0x89, 0x00));
            Colors.TryAdd((int)ForestTheme.HighlightA5, Color.FromRgb(0x96, 0x61, 0x00));

            Colors.TryAdd((int)ForestTheme.HighlightB1, Color.FromRgb(0xe5, 0xe9, 0xaf));
            Colors.TryAdd((int)ForestTheme.HighlightB2, Color.FromRgb(0xd1, 0xd9, 0x72));
            Colors.TryAdd((int)ForestTheme.HighlightB3, Color.FromRgb(0xbe, 0xc9, 0x36));
            Colors.TryAdd((int)ForestTheme.HighlightB4, Color.FromRgb(0x98, 0xa1, 0x2b));
            Colors.TryAdd((int)ForestTheme.HighlightB5, Color.FromRgb(0x72, 0x79, 0x20));

            Colors.TryAdd((int)ForestTheme.HighlightC1, Color.FromRgb(0xbf, 0xb8, 0xe9));
            Colors.TryAdd((int)ForestTheme.HighlightC2, Color.FromRgb(0x95, 0x89, 0xdb));
            Colors.TryAdd((int)ForestTheme.HighlightC3, Color.FromRgb(0x6a, 0x5a, 0xcd));
            Colors.TryAdd((int)ForestTheme.HighlightC4, Color.FromRgb(0x48, 0x36, 0xb3));
            Colors.TryAdd((int)ForestTheme.HighlightC5, Color.FromRgb(0x35, 0x28, 0x84));


            Colors.TryAdd((int)ForestTheme.Danger100, Color.FromRgb(0xbb, 0x12, 0x14));
            Colors.TryAdd((int)ForestTheme.Danger200, Color.FromRgb(0xd5, 0x15, 0x17));
            Colors.TryAdd((int)ForestTheme.Danger300, Color.FromRgb(0x99, 0x0f, 0x10));
            
            Colors.TryAdd((int)ForestTheme.Warning100, Color.FromRgb(0xe3, 0xa2, 0x1a));
            Colors.TryAdd((int)ForestTheme.Warning100, Color.FromRgb(0xff, 0xb6, 0x1d));
            Colors.TryAdd((int)ForestTheme.Warning100, Color.FromRgb(0xbf, 0x88, 0x16));
            
            Colors.TryAdd((int)ForestTheme.Info100, Color.FromRgb(0x00, 0x7a, 0xcc));
            Colors.TryAdd((int)ForestTheme.Info100, Color.FromRgb(0x00, 0x92, 0xf2));
            Colors.TryAdd((int)ForestTheme.Info100, Color.FromRgb(0x00, 0x62, 0xa3));
            
            Colors.TryAdd((int)ForestTheme.Success100, Color.FromRgb(0x99, 0xb4, 0x33));
            Colors.TryAdd((int)ForestTheme.Success100, Color.FromRgb(0xb6, 0xd6, 0x3d));
            Colors.TryAdd((int)ForestTheme.Success100, Color.FromRgb(0x82, 0x99, 0x2f));
            
            Colors.TryAdd((int)ForestTheme.Obsolete100, Color.FromRgb(0xca, 0x51, 0x00));
            Colors.TryAdd((int)ForestTheme.Obsolete100, Color.FromRgb(0xf5, 0x62, 0x00));
            Colors.TryAdd((int)ForestTheme.Obsolete100, Color.FromRgb(0xb2, 0x48, 0x00));
        }
    }
}