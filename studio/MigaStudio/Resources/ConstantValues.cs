using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public class ConstantValues
    {
        public static MainTheme[] Themes => new[] { MainTheme.Light, MainTheme.Dark };
        public static CultureArea[] Languages => new[]
        {
            CultureArea.Chinese, 
            CultureArea.English, 
            CultureArea.Japanese, 
            CultureArea.Russian, 
            CultureArea.Korean, 
            CultureArea.French
        };
    }
}