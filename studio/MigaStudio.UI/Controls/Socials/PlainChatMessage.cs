namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class PlainChatMessage : UserMessageBase
    {
        static PlainChatMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainChatMessage), new FrameworkPropertyMetadata(typeof(PlainChatMessage)));
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(string),
            typeof(PlainChatMessage),
            new PropertyMetadata(default(string)));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        
    }
}