using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using NLog.Targets.Wrappers;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class AvatarConverter : IValueConverter
    {
        private static readonly ConcurrentDictionary<string, ImageSource> Pool = new ConcurrentDictionary<string, ImageSource>();
        private static          ImageEngine                               _engine;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDataCache cache)
            {
                if (string.IsNullOrEmpty(cache.Avatar))
                {
                    // TODO
                }
                else
                {
                    return Caching(cache.Avatar);
                }
            }

            return null;
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}