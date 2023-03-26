using System;
using System.IO;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class TemplateSystem
    {
        public static void InstallLanguages()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "TemplateSystem.en.ini",
                CultureArea.French   => "TemplateSystem.fr.ini",
                CultureArea.Japanese => "TemplateSystem.jp.ini",
                CultureArea.Korean   => "TemplateSystem.kr.ini",
                CultureArea.Russian  => "TemplateSystem.ru.ini",
                _                    => "TemplateSystem.cn.ini",
            };
            
            Language.AppendLanguageSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", fileName));
        }
        
        public static void InstallViews()
        {
            
            Xaml.InstallView<EditBlockView, EditBlockViewModel>();
            Xaml.InstallView<NewBlockView, NewBlockViewModel>();
            Xaml.InstallView<NewElementView, NewElementViewModel>();
            Xaml.InstallView<TemplateEditorPage, TemplateEditorViewModel>();
        }
    }
}