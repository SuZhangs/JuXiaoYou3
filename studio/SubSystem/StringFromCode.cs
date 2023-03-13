using System;
using System.IO;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class StringFromCode
    {

        static StringFromCode()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "SubSystem.en.ini",
                CultureArea.French   => "SubSystem.fr.ini",
                CultureArea.Japanese => "SubSystem.jp.ini",
                CultureArea.Korean   => "SubSystem.kr.ini",
                CultureArea.Russian  => "SubSystem.ru.ini",
                _                    => "SubSystem.cn.ini",
            };
            Language.AppendLanguageSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", fileName));
        }

        public static string GetText(string id) => Language.GetText(id);
        
        
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
        
        public static string GetEngineResult(EngineFailedReason reason)
        {
            // TODO: 翻译
            return reason switch
            { 
                EngineFailedReason.Duplicated => GetText("enum.EngineFailedReason.Duplicated"),
                _ => GetText("enum.EngineFailedReason.Unknown")
            };
        }

        public static string Notify => GetText("dialog.title.notify");
        public static string BadImage => GetText("text.notimage");
        public static string ImageTooSmall => GetText("text.ImageTooSmall");
        public static string ImageTooBig => GetText("text.ImageTooBig");
        
        public static string OperationOfAddIsSuccess => GetText("text.OperationOfAddIsSuccess");
        public static string ImageProcessing
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Processing...",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "正在处理图片....",
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

        public static string EmptyName => GetText("text.EmptyName");
    }
}