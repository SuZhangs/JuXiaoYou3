using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog.Targets.Wrappers;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AvatarConverter : IMultiValueConverter
    {
        private static readonly ConcurrentDictionary<string, ImageSource> Pool = new ConcurrentDictionary<string, ImageSource>();
        private static          ImageEngine                               _engine;

        private static readonly BitmapImage _character = new BitmapImage(new Uri("pack://application:,,,/Forest.Fonts;component/avatar.png"));

        private static ImageSource FallbackImage(DocumentType type)
        {
            return _character;
        }

        private static ImageSource Caching(string avatar)
        {
            _engine ??= Xaml.Get<IDatabaseManager>()
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
                return FallbackImage(DocumentType.Document);
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

            return FallbackImage(DocumentType.Document);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
                return FallbackImage(DocumentType.Document);
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

            return FallbackImage(DocumentType.Document);
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
            _engine ??= Xaml.Get<IDatabaseManager>()
                            .GetEngine<ImageEngine>();
            var ms = _engine.Get(avatar);
            var img = Xaml.FromStream(ms, 1920, 1080);
            return img;
        }
        private static ImageSource Caching(string avatar, int w, int h)
        {
            _engine ??= Xaml.Get<IDatabaseManager>()
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
}