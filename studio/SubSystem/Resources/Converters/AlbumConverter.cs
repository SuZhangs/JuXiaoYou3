using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AlbumConverter : IValueConverter
    {
        private static           MusicEngine _engine;
        internal static readonly BitmapImage _album = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/album.png"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _engine ??= Studio.DatabaseManager()
                            .GetEngine<MusicEngine>();

            var album = value?.ToString();

            if (string.IsNullOrEmpty(album))
            {
                return _album;
            }

            var ms = _engine.Get(album);
            if (ms is null)
            {
                return _album;
            }
            return Xaml.FromStream(ms, 256, 256);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}