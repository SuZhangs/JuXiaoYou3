

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class StringFromCode
    {
        public static string GetImageFilter()
        {
            // TODO: 翻译
            return Language.Culture switch
            {
                CultureArea.English => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _ => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }
        
        public static string GetModuleFilter()
        {
            // TODO: 翻译
            return Language.Culture switch
            {
                CultureArea.English => "Module File|*.png",
                CultureArea.French => "Module File|*.png",
                CultureArea.Japanese => "Module File|*.png",
                CultureArea.Korean => "Module File|*.png",
                CultureArea.Russian => "Module File|*.png",
                _ => "模组文件|*.png",
            };
        }
    }
}