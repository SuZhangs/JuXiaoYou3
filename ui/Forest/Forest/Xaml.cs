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
    }
}