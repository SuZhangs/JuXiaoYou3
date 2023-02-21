using System.Diagnostics;
using System.IO;
using System.Windows;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Interfaces;
// ReSharper disable InlineOutVariableDeclaration

namespace Acorisoft.FutureGL.Forest
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
            var    resolver = Xaml.Get<ILanguageNodeResolver>();
            var    node     = resolver.GetNode(element);
            var    textKey  = $"{id}.Text";
            var    toolKey  = $"{id}.ToolTips";
            string text;

            if (GlobalStrings.TryGetValue(textKey, out text))
            {
                node.SetText(text);
            }
                
            if (GlobalStrings.TryGetValue(toolKey, out text))
            {
                node.SetToolTips(text);
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
                #if DEBUG
                Debug.WriteLine($"根元素:{value}已经应用本地化翻译服务");
                #endif
                
                vmls.RootName    =  value;
            }

            element.SetValue(RootProperty, value);
        }

        public static string GetRoot(FrameworkElement element)
        {
            return (string)element.GetValue(RootProperty);
        }
        

        #endregion

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
                var    id       = $"{vmls.RootName}.{value}";
                var    resolver = Xaml.Get<ILanguageNodeResolver>();
                var    node     = resolver.GetNode(element);
                var    textKey  = $"{id}.Text";
                var    toolKey  = $"{id}.ToolTips";
                string text;

                if (GlobalStrings.TryGetValue(textKey, out text))
                {
                    node.SetText(text);
                }
                
                if (GlobalStrings.TryGetValue(toolKey, out text))
                {
                    node.SetToolTips(text);
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
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void SetLanguage(string fileName)
        {
            // pageRoot.Id.Function
            try
            {
                var lines = File.ReadAllLines(fileName);
                var temp = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var line in lines)
                {
                    var separator = line.IndexOf('=');
                    var id = line[..separator].Trim();
                    var value = line[(separator + 1)..].Trim();

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

                    _ => "拒绝"
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

                    _ => "放弃"
                };
            }
        }
    }
}