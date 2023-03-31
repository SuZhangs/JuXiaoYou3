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
using NLog.Targets.Wrappers;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AvatarConverter : IMultiValueConverter
    {
        private static readonly ConcurrentDictionary<string, ImageSource> Pool = new ConcurrentDictionary<string, ImageSource>();
        private static          ImageEngine                               _engine;
        private static readonly BitmapImage                               _character = new BitmapImage(new Uri("pack://application:,,,/SubSystem;component/assets/avatar.png"));
        
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
}