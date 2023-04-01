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
            return SubSystemString.GetDocumentTypeName(value is DocumentType t ? t : DocumentType.OtherDocument);
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}