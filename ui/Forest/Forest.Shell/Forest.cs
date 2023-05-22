using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Services;
using DryIoc;

// ReSharper disable ForCanBeConvertedToForeach

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// <see cref="Xaml"/> 类型表示一个Xaml帮助类。
    /// </summary>
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public static class UIHelper
    {
        private static readonly Lazy<IDialogService> DialogServiceField = new Lazy<IDialogService>(Xaml.Get<IDialogService>);

        


        public static IDialogService DialogService()
        {
            return DialogServiceField.Value;
        }

        #region Notify

        static string DangerOperationCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Dangerous Operation",
                    CultureArea.Korean   => "위험한 조작",
                    CultureArea.Japanese => "危険な操作です",
                    CultureArea.French   => "Une opération dangereuse",
                    CultureArea.Russian  => "Опасная операция",
                    _                    => "危险操作"
                };
            }
        }

        static string SensitiveOperationCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Sensitive operation",
                    CultureArea.Korean   => "민감한 조작",
                    CultureArea.Japanese => "デリケートな操作です",
                    CultureArea.French   => "Une opération sensible",
                    CultureArea.Russian  => "Деликатная операция",
                    _                    => "敏感操作"
                };
            }
        }

        static string ErrorCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Error",
                    CultureArea.Korean   => "오류",
                    CultureArea.Japanese => "間違いです",
                    CultureArea.French   => "Les erreurs",
                    CultureArea.Russian  => "ошибк",
                    _                    => "错误"
                };
            }
        }

        static string WarningCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Warning",
                    CultureArea.Korean   => "경고",
                    CultureArea.Japanese => "警告します",
                    CultureArea.French   => "Avertissement",
                    CultureArea.Russian  => "предупред",
                    _                    => "警告"
                };
            }
        }

        static string InfoCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Error",
                    CultureArea.Korean   => "오류",
                    CultureArea.Japanese => "間違いです",
                    CultureArea.French   => "Les erreurs",
                    CultureArea.Russian  => "ошибк",
                    _                    => "错误"
                };
            }
        }

        static string SuccessfulCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Successful",
                    CultureArea.Korean   => "성공",
                    CultureArea.Japanese => "成功です",
                    CultureArea.French   => "Le succès",
                    CultureArea.Russian  => "успешн",
                    _                    => "成功"
                };
            }
        }

        static string ObsoletedCaption
        {
            get
            {
                return Language.Culture switch
                {
                    CultureArea.English  => "Obsoleted",
                    CultureArea.Korean   => "기한이 지나면",
                    CultureArea.Japanese => "期限切れです",
                    CultureArea.French   => "Périmés",
                    CultureArea.Russian  => "Срок годност",
                    _                    => "过期"
                };
            }
        }



        #endregion
        
        
        public static Task<bool> Error(this IViewModel source, string content)
        {
            return DialogService()
                .Danger(DangerOperationCaption, content);
        }
        
        public static Task<bool> Error(this IViewModel source, string content, int seconds)
        {
            return DialogService()
                .Danger(DangerOperationCaption, content, seconds);
        }

        public static Task<bool> Warning(this IViewModel source, string content)
        {
            return DialogService()
                .Warning(SensitiveOperationCaption, content);
        }
        
        public static Task<bool> Warning(this IViewModel source, string content, int seconds)
        {
            return DialogService()
                .Warning(SensitiveOperationCaption, content, seconds);
        }


        public static Task Obsoleted(this IViewModel source, string content)
        {
            return DialogService()
                .Notify(CriticalLevel.Obsoleted, ObsoletedCaption, content);
        }
        
        public static Task Obsoleted(this IViewModel source, string content, int seconds)
        {
            return DialogService()
                .Notify(CriticalLevel.Obsoleted, ObsoletedCaption, content,seconds);
        }

        public static void Successful(this IViewModel source, string content, int seconds = 2)
        {
            Xaml.Get<INotifyService>()
                .Notify(new IconNotification
                {
                    Color = ThemeSystem.Instance
                                       .Theme
                                       .Colors[(int)ForestTheme.Success100],
                    Delay    = TimeSpan.FromSeconds(seconds),
                    Geometry = Xaml.Checked,
                    IsFilled = false,
                    Title    = content
                });
        }

        public static Task WarningNotification(this IViewModel source, string content)
        {
            return DialogService()
                .Notify(CriticalLevel.Warning, WarningCaption, content);
        }

        public static void Info(this IViewModel source, string content, int seconds = 2)
        {
            Xaml.Get<INotifyService>()
                .Notify(new IconNotification
                {
                    Color = ThemeSystem.Instance
                                       .Theme
                                       .Colors[(int)ForestTheme.Info100],
                    Delay    = TimeSpan.FromSeconds(seconds),
                    Geometry = Xaml.InfoGeometry,
                    IsFilled = false,
                    Title    = content
                });
        }

        public static void ErrorNotification(this IViewModel source, string content, int seconds = 2)
        {
            Xaml.Get<INotifyService>()
                .Notify(new IconNotification
                {
                    Color = ThemeSystem.Instance
                                       .Theme
                                       .Colors[(int)ForestTheme.Danger100],
                    Delay    = TimeSpan.FromSeconds(seconds),
                    Geometry = Xaml.ErrorGeometry,
                    IsFilled = false,
                    Title    = content
                });
        }
    }
}