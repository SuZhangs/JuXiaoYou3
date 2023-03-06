

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public class ConstantValues
    {
        public static object[] Themes => new object[] { MainTheme.Light, MainTheme.Dark };
        public static object[] Languages => new object[]
        {
            CultureArea.Chinese, 
            CultureArea.English, 
            CultureArea.Japanese, 
            CultureArea.Russian, 
            CultureArea.Korean, 
            CultureArea.French
        };


        internal const string Setting_MainTheme = "setting.theme";
        internal const string Setting_Language = "setting.language";
        internal const string PageName_Home     = "global.home";
        internal const string PageName_Setting  = "global.setting";
    }
}