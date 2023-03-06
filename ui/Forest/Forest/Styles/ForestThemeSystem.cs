using System.Windows;
using System.Collections.Concurrent;

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

        public Color GetCallToActionColor(HighlightColorPalette palette, int level)
        {
            return level switch
            {
                2 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB4],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC4],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger200],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning200],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info200],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success200],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete200],
                    _                                       => Colors[(int)ForestTheme.HighlightA4],
                },
                3 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB5],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC5],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger300],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning300],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info300],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success300],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete300],
                    _                                       => Colors[(int)ForestTheme.HighlightA5],
                },
                _ => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB3],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC3],
                    HighlightColorPalette.Danger            => Colors[(int)ForestTheme.Danger100],
                    HighlightColorPalette.Warning           => Colors[(int)ForestTheme.Warning100],
                    HighlightColorPalette.Info              => Colors[(int)ForestTheme.Info100],
                    HighlightColorPalette.Success           => Colors[(int)ForestTheme.Success100],
                    HighlightColorPalette.Obsolete          => Colors[(int)ForestTheme.Obsolete100],
                    _                                       => Colors[(int)ForestTheme.HighlightA3],
                }
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
                    _                                       => Colors[(int)ForestTheme.HighlightA2],
                },
                3 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB3],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC3],
                    _                                       => Colors[(int)ForestTheme.HighlightA3],
                },
                4 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB4],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC4],
                    _                                       => Colors[(int)ForestTheme.HighlightA4],
                },
                5 => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB5],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC5],
                    _                                       => Colors[(int)ForestTheme.HighlightA5],
                },
                _ => palette switch
                {
                    HighlightColorPalette.HighlightPalette2 => Colors[(int)ForestTheme.HighlightB1],
                    HighlightColorPalette.HighlightPalette3 => Colors[(int)ForestTheme.HighlightC1],
                    _                                       => Colors[(int)ForestTheme.HighlightA1],
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