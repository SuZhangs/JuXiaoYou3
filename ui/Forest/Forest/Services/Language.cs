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

        public static readonly DependencyProperty ElementsProperty = DependencyProperty.RegisterAttached(
            "ElementsOverride",
            typeof(Dictionary<string, ILanguageNode>), 
            typeof(Language), 
            new PropertyMetadata(default(Dictionary<string, ILanguageNode>)));

        #region Elements
        

        private static void SetElements(DependencyObject element, Dictionary<string, ILanguageNode> value)
        {
            value ??= new Dictionary<string, ILanguageNode>();
            element.SetValue(ElementsProperty, value);
        }

        private static Dictionary<string, ILanguageNode> GetElements(DependencyObject element)
        {
            return (Dictionary<string, ILanguageNode>)element.GetValue(ElementsProperty);
        }

        #endregion

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
            if (element.DataContext is IViewModelLanguageService vmls)
            {
                var resolver = Xaml.Get<ILanguageNodeResolver>();

                vmls.ElementBag.Add(id, resolver.GetNode(element));
                element.SetValue(IdProperty, id);
            } 
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
                
                vmls.ElementBag  =  new Dictionary<string, ILanguageNode>(StringComparer.OrdinalIgnoreCase);
                vmls.RootName    =  value;
                element.Loaded   += StartTranslate;
                element.Unloaded -= StartTranslate;
            }

            element.SetValue(RootProperty, value);
        }

        private static void StartTranslate(object sender, RoutedEventArgs e)
        {
            var root     = (FrameworkElement)sender;
            var rootName = GetRoot(root);
            var elements = GetElements(root);

            if (elements is null)
            {
                return;
            }
            
            Debug.WriteLine($"根元素：{rootName} 开始本地化文本，总元素个数:{elements.Count}");

            foreach (var (key, resolver) in elements)
            {
                var    textKey = $"{key}.Text";
                var    toolKey = $"{key}.ToolTips";
                string text;

                if (GlobalStrings.TryGetValue(textKey, out text))
                {
                    resolver.SetText(text);
                }
                
                if (GlobalStrings.TryGetValue(toolKey, out text))
                {
                    resolver.SetToolTips(text);
                }
            }
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
                var id       = $"{value}.{vmls.RootName}";
                var resolver = Xaml.Get<ILanguageNodeResolver>();
                vmls.ElementBag.Add(id, resolver.GetNode(element));
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
                    var line2 = line.Trim();
                    var separator = line2.IndexOf('=');
                    var id = line2[..separator];
                    var value = line2[separator..];

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