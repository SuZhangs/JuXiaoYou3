using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageViewModel : DialogViewModel
    {
        protected override void OnStart(RoutingEventArgs parameter)
        {
            var fileName = parameter.Parameter
                                    .Args[0]
                                    ?.ToString();
            Source = new BitmapImage(new Uri(fileName, UriKind.Absolute));
            base.OnStart(parameter);
        }

        private ImageSource _source;

        /// <summary>
        /// 获取或设置 <see cref="Source"/> 属性。
        /// </summary>
        public ImageSource Source
        {
            get => _source;
            set => SetValue(ref _source, value);
        }
    }
}