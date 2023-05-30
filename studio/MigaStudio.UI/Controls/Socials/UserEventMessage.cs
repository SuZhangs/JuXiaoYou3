namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class UserEventMessage : ChatMessageBase
    {
        static UserEventMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserEventMessage), new FrameworkPropertyMetadata(typeof(UserEventMessage)));
        }
        
        
        public static readonly DependencyProperty CharacterNameProperty = DependencyProperty.Register(
            nameof(CharacterName),
            typeof(string),
            typeof(UserEventMessage),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(string),
            typeof(UserEventMessage),
            new PropertyMetadata(default(string)));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public string CharacterName
        {
            get => (string)GetValue(CharacterNameProperty);
            set => SetValue(CharacterNameProperty, value);
        }
    }
}