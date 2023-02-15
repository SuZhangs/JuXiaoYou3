using System.Windows;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;

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
        }
        
        
        
        /// <summary>
        /// 边框的定义
        /// </summary>
        public Bag<Thickness> BorderThickness { get; protected init; }

        /// <summary>
        /// 圆角的定义
        /// </summary>
        public Bag<Thickness> CornerRadius { get; protected init; }
        
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