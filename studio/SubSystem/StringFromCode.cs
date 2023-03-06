using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class StringFromCode
    {
        public static string GetDatabaseResult(DatabaseFailedReason reason)
        {
            // TODO: 翻译
            return reason switch
            { 
                DatabaseFailedReason.DatabaseNotOpen => Language.Culture switch
                {
                    CultureArea.English  => "Database Not open!",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "还没有打开任何世界观设定！",
                },
                _ => Language.Culture switch
                {
                    CultureArea.English  => "Unknown error, can you feedback for us?",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "未知的错误，能否联系开发者反馈Bug？谢谢！",
                }
            };
        }
        
        public static string Notify
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Notice",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "提示",
            };
        }
        
        public static string OperationOfAddIsSuccess
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Add document successful!",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "添加成功！",
            };
        }
        
        public static string GetEngineResult(EngineFailedReason reason)
        {
            // TODO: 翻译
            return reason switch
            { 
                EngineFailedReason.Duplicated => Language.Culture switch
                {
                    CultureArea.English  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
                },
                _ => Language.Culture switch
                {
                    CultureArea.English  => "Unknown error, can you feedback for us?",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "未知的错误，能否联系开发者反馈Bug？谢谢！",
                }
            };
        }
        
        public static string ImageFilter
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _ => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }
        
        public static string GetModuleFilter
        {
            // TODO: 翻译
            get => Language.Culture switch
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