using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Inputs;
using Acorisoft.FutureGL.Forest.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {
        protected PageViewModel()
        {
            KeyInputs = new List<KeyInput>(8);
            Xaml.Get<IWindowEventBroadcast>()
                .Keys
                .Subscribe(OnKeyDown)
                .DisposeWith(Collector);
        }

        private void OnKeyDown(WindowKeyEventArgs e)
        {
            if (KeyInputs.Count == 0)
            {
                return;
            }

            var modifier = e.Args.KeyboardDevice.Modifiers;
            var key      = e.Args.Key;

            foreach (var input in KeyInputs)
            {
                if (input.Modifiers == ModifierKeys.None && key == input.Key)
                {
                    input.Expression?.Invoke();
                }
                else if (key == input.Key && modifier == input.Modifiers)
                {
                    input.Expression?.Invoke();
                }
            }
        }

        protected void AddKeyBinding(Key key, Action expression)
        {
            if (expression is null)
            {
                return;
            }

            KeyInputs.Add(new KeyInput
            {
                Key        = key,
                Modifiers  = ModifierKeys.None,
                Expression = expression
            });
        }

        protected void AddKeyBinding(ModifierKeys modifier, Key key, Action expression)
        {
            if (expression is null)
            {
                return;
            }

            KeyInputs.Add(new KeyInput
            {
                Key        = key,
                Modifiers  = modifier,
                Expression = expression
            });
        }

        protected List<KeyInput> KeyInputs { get; }

        public override string ToString()
        {
            return $"{GetHashCode()}";
        }
    }
}