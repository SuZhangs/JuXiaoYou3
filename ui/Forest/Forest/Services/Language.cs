using System.Diagnostics;
using System.IO;
using System.Windows;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Interfaces;

// ReSharper disable InlineOutVariableDeclaration
// ReSharper disable SuspiciousTypeConversion.Global

namespace Acorisoft.FutureGL.Forest.Services
{
    public static class Language
    {
        private static readonly Dictionary<string, string> _stringDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
            "NameOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty RootProperty = DependencyProperty.RegisterAttached(
            "RootOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IdProperty = DependencyProperty.RegisterAttached(
            "IdOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        #region Id

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要添加的元素</param>
        /// <param name="id">值</param>
        /// <remarks>
        /// <para>默认情况下<see cref="IdProperty"/>存储的内容为: {root}.{name}</para>
        /// <para>但是您可以通过调用该方法指定值来覆盖默认设置。</para>
        /// </remarks>
        public static void SetId(FrameworkElement element, string id)
        {
            var    adapter = element as ITextResourceAdapter ?? Xaml.Get<ITextResourceFactory>().Adapt(element);
            var    toolKey = $"{id}.tips";
            string text;

            if (GlobalStrings.TryGetValue(id, out text))
            {
                adapter.SetText(text);
            }

            if (GlobalStrings.TryGetValue(toolKey, out text))
            {
                adapter.SetToolTips(text);
            }

            element.SetValue(IdProperty, id);
        }

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要获取的元素</param>
        /// <returns>返回用于翻译的元素键</returns>
        public static string GetId(FrameworkElement element)
        {
            return (string)element.GetValue(IdProperty);
        }

        #endregion

        #region Root

        public static void SetRoot(FrameworkElement element, string value)
        {
            if (element.DataContext is IViewModelLanguageService vmls)
            {
                vmls.RootName = value;
            }

            element.SetValue(RootProperty, value);
        }

        public static string GetRoot(FrameworkElement element)
        {
            return (string)element.GetValue(RootProperty);
        }

        #endregion

        public static string GetText(string id)
        {
            return GlobalStrings.TryGetValue(id, out var text) ? text : id;
        }
        
        #region Name

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要添加的元素</param>
        /// <param name="value">值</param>
        /// <remarks>
        /// <para>默认情况下<see cref="IdProperty"/>存储的内容为: {root}.{name}</para>
        /// <para>但是您可以通过调用该方法指定值来覆盖默认设置。</para>
        /// </remarks>
        public static void SetName(FrameworkElement element, string value)
        {
            //
            // Find RootName
            //
            if (element.DataContext is IViewModelLanguageService vmls)
            {
                var    id      = $"{vmls.RootName}.{value}";
                var    adapter = element as ITextResourceAdapter ?? Xaml.Get<ITextResourceFactory>().Adapt(element);
                var    textKey = $"{id}";
                var    toolKey = $"{id}.tips";
                string text;

                if (GlobalStrings.TryGetValue(textKey, out text))
                {
                    adapter.SetText(text);
                }

                if (GlobalStrings.TryGetValue(toolKey, out text))
                {
                    adapter.SetToolTips(text);
                }

                element.SetValue(IdProperty, id);
            }

            element.SetValue(NameProperty, value);
        }

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要获取的元素</param>
        /// <returns>返回用于翻译的元素键</returns>
        public static string GetName(FrameworkElement element)
        {
            return (string)element.GetValue(NameProperty);
        }

        #endregion

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void SetLanguage(string fileName)
        {
            // pageRoot.Id.Function
            try
            {
                var lines = File.ReadAllLines(fileName);
                var temp  = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var line in lines)
                {
                    var separator = line.IndexOf('=');
                    var id        = line[..separator].Trim();
                    var value     = line[(separator + 1)..].Trim();

                    temp.TryAdd(id, value);
                }

                _stringDictionary.Clear();

                foreach (var kv in temp)
                {
                    _stringDictionary.Add(kv.Key, kv.Value);
                }
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 全局内容文本
        /// </summary>
        public static IReadOnlyDictionary<string, string> GlobalStrings => _stringDictionary;

        /// <summary>
        /// 指定文化
        /// </summary>
        /// <remarks>语言的切换必须重启。</remarks>
        public static CultureArea Culture { get; set; }

        /// <summary>
        /// 确定
        /// </summary>
        public static string ConfirmText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Ok",
                    CultureArea.French => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean => "kr.ini",
                    CultureArea.Russian => "ru.ini",
                    _ => "确定"
                };
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        public static string RejectText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Reject",
                    CultureArea.French => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean => "kr.ini",
                    CultureArea.Russian => "ru.ini",
                    _ => "拒绝"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string NotifyText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Notify",
                    CultureArea.French   => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean   => "kr.ini",
                    CultureArea.Russian  => "ru.ini",
                    _                    => "提示"
                };
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        public static string CancelText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Cancel",
                    CultureArea.French => "Annuler",
                    CultureArea.Japanese => "取り消す",
                    CultureArea.Korean => "취소",
                    CultureArea.Russian => "Отмена",

                    _ => "放弃"
                };
            }
        }
    }
}