using System.Windows;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Styles
{
    public abstract class ForestThemeSystem : ForestObject
    {
        protected ForestThemeSystem()
        {
            Colors     = new ConcurrentDictionary<int, Color>();
            Geometries = new ConcurrentDictionary<int, Geometry>();
            Images     = new ConcurrentDictionary<int, ImageSource>();
            CornerRadius = new Bag<Thickness>
            {
                Samll  = new Thickness(8),
                Medium = new Thickness(12),
                Large  = new Thickness(24)
            };

            BorderThickness = new Bag<Thickness>
            {
                Samll  = new Thickness(2),
                Medium = new Thickness(4),
                Large  = new Thickness(8)
            };

            Duration = new Bag<Duration>
            {
                Samll  = new Duration(TimeSpan.FromMilliseconds(150)),
                Medium = new Duration(TimeSpan.FromMilliseconds(200)),
                Large  = new Duration(TimeSpan.FromMilliseconds(300)),
            };
        }

        public Color GetHighlightColor(HighlightColorPalette palette, int level)
        {
            return level switch
            {
                2 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB2],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC2],
                    _ => Colors[(int)ForestTheme.HighlightA2],
                },
                3 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB3],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC3],
                    _ => Colors[(int)ForestTheme.HighlightA3],
                },
                4 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB4],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC4],
                    _ => Colors[(int)ForestTheme.HighlightA4],
                },
                5 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB5],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC5],
                    _ => Colors[(int)ForestTheme.HighlightA5],
                },
                _ => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB1],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC1],
                    _ => Colors[(int)ForestTheme.HighlightA1],
                }
            };
        }
        
        public Bag<Duration> Duration { get; }

        /// <summary>
        /// 边框的定义
        /// </summary>
        public Bag<Thickness> BorderThickness { get; }

        /// <summary>
        /// 圆角的定义
        /// </summary>
        public Bag<Thickness> CornerRadius { get; }

        /// <summary>
        /// 颜色池
        /// </summary>
        public ConcurrentDictionary<int, Color> Colors { get; }

        /// <summary>
        /// 形状池
        /// </summary>
        public ConcurrentDictionary<int, Geometry> Geometries { get; }

        /// <summary>
        /// 图形池
        /// </summary>
        public ConcurrentDictionary<int, ImageSource> Images { get; }
    }
}