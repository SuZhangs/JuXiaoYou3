using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Xaml.Use<ForestResourceFactory, ITextResourceFactory>(new ForestResourceFactory());
            ThemeSystem.Instance.Theme = new ForestLightTheme();
            Language.Culture           = CultureArea.English;
            Language.SetLanguage(GetLanguageFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages")));
        }
        
        protected static string GetLanguageFile(string appModel)
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "en.ini",
                CultureArea.French   => "fr.ini",
                CultureArea.Japanese => "jp.ini",
                CultureArea.Korean   => "kr.ini",
                CultureArea.Russian  => "ru.ini",
                _                    => "cn.ini",
            };
            
            return Path.Combine(appModel, fileName);
        }
    }
}