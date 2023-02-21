using System.Windows;
using System.Windows.Controls.Primitives;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest
{
    public class LanguageNodeResolver : ILanguageNodeResolver
    {
        public class ContentControlResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Content = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public ContentControl Target { get; init; }
        }

        public class HeaderedContentControlResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Header = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public HeaderedContentControl Target { get; init; }
        }

        public class TextBoxResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Text = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public TextBox Target { get; init; }
        }

        public class WindowResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Title = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public Window Target { get; init; }
        }

        public class TextBlockResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Text = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public TextBlock Target { get; init; }
        }

        public class DefaultResolver : ILanguageNode
        {
            public void SetText(string text)
            {
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public FrameworkElement Target { get; init; }
        }

        public class MenuItemResolver : ILanguageNode
        {
            public void SetText(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.Header = text;
            }

            public void SetToolTips(string text)
            {
                if (Target is null)
                {
                    return;
                }

                Target.ToolTip = text;
            }

            public MenuItem Target { get; init; }
        }

        public LanguageNodeResolver()
        {
            Factory = new Dictionary<Type, Func<FrameworkElement, ILanguageNode>>
            {
                { typeof(Label), x => new ContentControlResolver { Target                          = (ContentControl)x } },
                { typeof(Button), x => new ContentControlResolver { Target                         = (ContentControl)x } },
                { typeof(RadioButton), x => new ContentControlResolver { Target                    = (ContentControl)x } },
                { typeof(RepeatButton), x => new ContentControlResolver { Target                   = (ContentControl)x } },
                { typeof(ToggleButton), x => new ContentControlResolver { Target                   = (ContentControl)x } },
                { typeof(CheckBox), x => new ContentControlResolver { Target                       = (ContentControl)x } },
                { typeof(ContentControl), x => new ContentControlResolver { Target                 = (ContentControl)x } },
                { typeof(GroupItem), x => new ContentControlResolver { Target                      = (ContentControl)x } },
                { typeof(Window), x => new WindowResolver { Target                                 = (Window)x } },
                { typeof(GroupBox), x => new HeaderedContentControlResolver { Target               = (HeaderedContentControl)x } },
                { typeof(HeaderedContentControl), x => new HeaderedContentControlResolver { Target = (HeaderedContentControl)x } },
                { typeof(Expander), x => new HeaderedContentControlResolver() { Target             = (HeaderedContentControl)x } },
                { typeof(TabItem), x => new HeaderedContentControlResolver() { Target              = (HeaderedContentControl)x } },
                { typeof(ForestTabItem), x => new HeaderedContentControlResolver { Target          = (HeaderedContentControl)x } },
                { typeof(TextBox), x => new TextBoxResolver() { Target                             = (TextBox)x } },
                { typeof(ForestTextBox), x => new TextBoxResolver() { Target                       = (TextBox)x } },
                { typeof(TextBlock), x => new TextBlockResolver() { Target                         = (TextBlock)x } },
                { typeof(MenuItem), x => new MenuItemResolver() { Target                           = (MenuItem)x } },
            };
        }

        public ILanguageNode GetNode(FrameworkElement instance)
        {
            return Factory.TryGetValue(instance.GetType(), out var factory) ? factory(instance) : new DefaultResolver { Target = instance };
        }

        public Dictionary<Type, Func<FrameworkElement, ILanguageNode>> Factory { get; }
    }
}