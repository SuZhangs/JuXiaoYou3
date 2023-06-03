using System.Collections.Concurrent;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AvatarConverter : IMultiValueConverter, IValueConverter
    {
        private static readonly ConcurrentDictionary<string, ImageSource> Pool = new ConcurrentDictionary<string, ImageSource>();
        private static          ImageEngine                               _engine;
        public static readonly BitmapImage                               Character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));

        public void Reset() => Pool.Clear();
        
        private static ImageSource FallbackImage(DocumentType type)
        {
            return Character;
        }

        private static ImageSource Caching(string avatar)
        {
            _engine ??= Studio.DatabaseManager()
                            .GetEngine<ImageEngine>();

            if (!Pool.TryGetValue(avatar, out var img))
            {
                var ms = _engine.Get(avatar);
                img = Xaml.FromStream(ms, 256, 256);
                Pool.TryAdd(avatar, img);
            }

            return img;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return FallbackImage(DocumentType.Character);
            }

            if (values[1] is DocumentType type)
            {
                var avatar = values[0]?.ToString();
                if (string.IsNullOrEmpty(avatar))
                {
                    return FallbackImage(type);
                }


                return Dispatcher.CurrentDispatcher.Invoke(() => Caching(avatar));
            }

            return FallbackImage(DocumentType.Character);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return FallbackImage(DocumentType.Character);
            }
            
            var avatar = value?.ToString();
            
            
            if (string.IsNullOrEmpty(avatar))
            {
                return FallbackImage(DocumentType.Character);
            }

            return Dispatcher.CurrentDispatcher.Invoke(() => Caching(avatar));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    
    
    public class DirectiveAvatarConverter : IMultiValueConverter
    {

        private static readonly BitmapImage _character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));

        private static ImageSource FallbackImage(DocumentType type)
        {
            return _character;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return FallbackImage(DocumentType.Character);
            }

            if (values[1] is DocumentType type)
            {
                var avatar = values[0]?.ToString();
                if (string.IsNullOrEmpty(avatar))
                {
                    return FallbackImage(type);
                }


                return new BitmapImage(new Uri(avatar));
            }

            return FallbackImage(DocumentType.Character);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ImageConverter : IValueConverter
    {
        private static          ImageEngine                               _engine;

        private static ImageSource Caching(string avatar)
        {
            _engine ??= Studio.DatabaseManager()
                            .GetEngine<ImageEngine>();
            var ms = _engine.Get(avatar);
            var img = Xaml.FromStream(ms, 1920, 1080);
            return img;
        }
        
        private static ImageSource Caching(string avatar, int w, int h)
        {
            _engine ??= Studio.DatabaseManager()
                            .GetEngine<ImageEngine>();
            var ms  = _engine.Get(avatar);
            var img = Xaml.FromStream(ms, w, h);
            return img;
        }
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            if (value is Album a)
            {
                return Caching(a.Source, a.Width, a.Height);
            }
            
            var avatar = value.ToString();
            return string.IsNullOrEmpty(avatar) ? null : Dispatcher.CurrentDispatcher.Invoke(() => Caching(avatar));
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    
    
    public class ScaledImageConverter : IValueConverter
    {
        private ImageEngine _engine;

        public ScaledImageConverter()
        {
        }

        private ImageSource Caching(string avatar, int w, int h)
        {
            var ms  = _engine.Get(avatar);
            var img = Xaml.FromStream(ms, w, h);
            return img;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            var v = value.ToString();
            
            if (string.IsNullOrEmpty(v))
            {
                return null;
            }

            _engine ??= Studio.DatabaseManager()
                              .GetEngine<ImageEngine>();
            if (value is Album a)
            {
                return Caching(a.Source, a.Width, a.Height);
            }

            var values = v.Split(":");

            if (values.Length < 3)
            {

                return null;
            }
            
            var avatar = values[0];
            var w      = int.TryParse(values[1], out var n) ? n : 100;
            var h      = int.TryParse(values[2], out n) ? n : 100;
            return string.IsNullOrEmpty(avatar) ? null : Dispatcher.CurrentDispatcher.Invoke(() => Caching($"{avatar}.png", w, h));
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}