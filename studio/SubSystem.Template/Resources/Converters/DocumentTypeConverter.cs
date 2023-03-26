using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class DocumentTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: 翻译
            if (value is DocumentType kind)
            {
                // TODO: 翻译
                return Language.Culture switch
                {
                    CultureArea.Chinese => GetChinese(kind),
                    _ => GetChinese(kind),
                };
            }

            return value?.ToString() ?? "Null";
        }


        private static string GetChinese(DocumentType kind)
        {
            return kind switch
            {
                DocumentType.AbilityDocument   => "能力",
                DocumentType.ItemDocument      => "物品",
                DocumentType.CharacterDocument => "人设",
                DocumentType.OtherDocument     => "其他",
                DocumentType.GeographyDocument => "地图",
                DocumentType.MysteryDocument   => "世界观设定",
                _                              => "人设",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}