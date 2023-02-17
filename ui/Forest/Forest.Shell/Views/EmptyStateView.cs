using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Views
{
    public abstract class EmptyStateView : UserControl
    {
        public static readonly DependencyProperty DefaultCommandProperty = DependencyProperty.Register(
            nameof(DefaultCommand),
            typeof(ICommand),
            typeof(EmptyStateView),
            new PropertyMetadata(default(ICommand)));

        public ICommand DefaultCommand
        {
            get => (ICommand)GetValue(DefaultCommandProperty);
            set => SetValue(DefaultCommandProperty, value);
        }
    }
}