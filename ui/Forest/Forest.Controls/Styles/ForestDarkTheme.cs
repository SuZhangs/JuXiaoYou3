﻿using System.Windows.Media;


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
            Colors.TryAdd((int)ForestTheme.HighlightA1, Color.FromRgb(0xe5, 0xe9, 0xaf));
            Colors.TryAdd((int)ForestTheme.HighlightA2, Color.FromRgb(0xd1, 0xd9, 0x72));
            Colors.TryAdd((int)ForestTheme.HighlightA3, Color.FromRgb(0xbe, 0xc9, 0x36));
            Colors.TryAdd((int)ForestTheme.HighlightA4, Color.FromRgb(0x98, 0xa1, 0x2b));
            Colors.TryAdd((int)ForestTheme.HighlightA5, Color.FromRgb(0x72, 0x79, 0x20));
            
            Colors.TryAdd((int)ForestTheme.HighlightB1, Color.FromRgb(0xff, 0xda, 0x96));
            Colors.TryAdd((int)ForestTheme.HighlightB2, Color.FromRgb(0xff, 0xc2, 0x54));
            Colors.TryAdd((int)ForestTheme.HighlightB3, Color.FromRgb(0xff, 0xab, 0x12));
            Colors.TryAdd((int)ForestTheme.HighlightB4, Color.FromRgb(0xd4, 0x89, 0x00));
            Colors.TryAdd((int)ForestTheme.HighlightB5, Color.FromRgb(0x96, 0x61, 0x00));

            Colors.TryAdd((int)ForestTheme.HighlightC1, Color.FromRgb(0xbf, 0xb8, 0xe9));
            Colors.TryAdd((int)ForestTheme.HighlightC2, Color.FromRgb(0x95, 0x89, 0xdb));
            Colors.TryAdd((int)ForestTheme.HighlightC3, Color.FromRgb(0x6a, 0x5a, 0xcd));
            Colors.TryAdd((int)ForestTheme.HighlightC4, Color.FromRgb(0x48, 0x36, 0xb3));
            Colors.TryAdd((int)ForestTheme.HighlightC5, Color.FromRgb(0x35, 0x28, 0x84));


            Colors.TryAdd((int)ForestTheme.Danger100, Color.FromRgb(0xbb, 0x21, 0x24));
            Colors.TryAdd((int)ForestTheme.Danger200, Color.FromRgb(0xd9, 0x08, 0x0c));
            Colors.TryAdd((int)ForestTheme.Danger300, Color.FromRgb(0xA6, 0x06, 0x09));
            Colors.TryAdd((int)ForestTheme.Danger400, Color.FromRgb(0xec, 0x93, 0x94));
            
            Colors.TryAdd((int)ForestTheme.Warning100, Color.FromRgb(0xdb, 0x94, 0x00));
            Colors.TryAdd((int)ForestTheme.Warning200, Color.FromRgb(0xff, 0xb3, 0x14));
            Colors.TryAdd((int)ForestTheme.Warning300, Color.FromRgb(0xa8, 0x72, 0x00));
            Colors.TryAdd((int)ForestTheme.Warning400, Color.FromRgb(0xff, 0xd6, 0x80));
            
            Colors.TryAdd((int)ForestTheme.Info100, Color.FromRgb(0x00, 0x7a, 0xcc));
            Colors.TryAdd((int)ForestTheme.Info200, Color.FromRgb(0x00, 0x92, 0xf2));
            Colors.TryAdd((int)ForestTheme.Info300, Color.FromRgb(0x00, 0x55, 0x8f));
            Colors.TryAdd((int)ForestTheme.Info400, Color.FromRgb(0x80, 0xcc, 0xff));
            
            Colors.TryAdd((int)ForestTheme.Success100, Color.FromRgb(0x99, 0xb4, 0x33));
            Colors.TryAdd((int)ForestTheme.Success200, Color.FromRgb(0xb2, 0xcc, 0x4c));
            Colors.TryAdd((int)ForestTheme.Success300, Color.FromRgb(0x76, 0x8b, 0x27));
            Colors.TryAdd((int)ForestTheme.Success400, Color.FromRgb(0xdd, 0xe8, 0xb0));
            
            Colors.TryAdd((int)ForestTheme.Obsolete100, Color.FromRgb(0xca, 0x51, 0x00));
            Colors.TryAdd((int)ForestTheme.Obsolete200, Color.FromRgb(0xff, 0x66, 0x00));
            Colors.TryAdd((int)ForestTheme.Obsolete300, Color.FromRgb(0x99, 0x3d, 0x00));
            Colors.TryAdd((int)ForestTheme.Obsolete400, Color.FromRgb(0xff, 0x94, 0x4d));
            
            
            Colors.TryAdd((int)ForestTheme.SlateGray100, Color.FromRgb(0x70, 0x80, 0x90));
            Colors.TryAdd((int)ForestTheme.SlateGray200, Color.FromRgb(0x4f, 0x5b, 0x66));
            Colors.TryAdd((int)ForestTheme.SlateGray300, Color.FromRgb(0x28, 0x2e, 0x34));
            Colors.TryAdd((int)ForestTheme.SlateGray400, Color.FromRgb(0x9b, 0xa6, 0xb1));
            
            
            Colors.TryAdd((int)ForestTheme.Mask100, Color.FromArgb(0x28, 0xff, 0xff, 0xff));
            Colors.TryAdd((int)ForestTheme.Mask200, Color.FromArgb(0x50, 0xff, 0xff, 0xff));
            Colors.TryAdd((int)ForestTheme.BorderBrush, Color.FromRgb( 0x77, 0x7c, 0x88));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel1, Color.FromRgb( 0x5f, 0x63, 0x6d));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel2, Color.FromRgb( 0x4a, 0x47, 0x52));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel3, Color.FromRgb( 0x43, 0x45, 0x4c));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel4, Color.FromRgb( 0x32, 0x34, 0x39));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel5, Color.FromRgb( 0x26, 0x28, 0x2b));
            Colors.TryAdd((int)ForestTheme.BackgroundLevel6, Color.FromRgb( 0x18, 0x19, 0x1b));
            Colors.TryAdd((int)ForestTheme.BackgroundDisabled, Color.FromRgb( 0x3c, 0x3e, 0x44));
            Colors.TryAdd((int)ForestTheme.ForegroundLevel1, Color.FromRgb( 0xe0, 0xe0, 0xe0));
            Colors.TryAdd((int)ForestTheme.ForegroundLevel2, Color.FromRgb( 0xc1, 0xc1, 0xbe));
            Colors.TryAdd((int)ForestTheme.ForegroundLevel3, Color.FromRgb( 0xa8, 0xa8, 0xa4));
            Colors.TryAdd((int)ForestTheme.ForegroundOther, Color.FromRgb( 0x84, 0x92, 0xa6));
            Colors.TryAdd((int)ForestTheme.ForegroundInHighlight, Color.FromRgb( 0xe0, 0xe0, 0xe0));
            Colors.TryAdd((int)ForestTheme.ForegroundDisabled, Color.FromRgb( 0x75, 0x75, 0x70));
        }
    }
}