using System.Windows.Media;
using DryIoc;

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// <see cref="Xaml"/> 类型表示一个Xaml帮助类。
    /// </summary>
    public static partial class Xaml
    {
        static Xaml()
        {
            Container = new Container(Rules.Default.WithTrackingDisposableTransients());
        }

        /// <summary>
        /// 转化为纯色画刷
        /// </summary>
        /// <param name="color">要转换的颜色</param>
        /// <returns>返回纯色画刷</returns>
        public static SolidColorBrush ToSolidColorBrush(this Color color) => new SolidColorBrush(color);
    }
}