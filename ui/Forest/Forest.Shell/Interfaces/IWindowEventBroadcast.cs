using System.Reactive.Subjects;
using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IWindowEventBroadcast
    {
        /// <summary>
        /// 键盘事件
        /// </summary>
        IObservable<WindowKeyEventArgs> Keys { get; }
    }

    public interface IWindowEventBroadcastAmbient
    {
        void SetEventSource(Window window);
    }

    public class WindowKeyEventArgs : EventArgs
    {
        public KeyEventArgs Args { get; init; }
        public bool IsDown { get; init; }
    }

    public class WindowEventBroadcast : IWindowEventBroadcast, IWindowEventBroadcastAmbient
    {
        private readonly Subject<WindowKeyEventArgs> _keys = new Subject<WindowKeyEventArgs>();

        private Window _eventSource;

        public void SetEventSource(Window window)
        {
            if (window is null)
            {
                return;
            }
            
            if (_eventSource is not null)
            {
                _eventSource.KeyUp -= OnKeyDown;
                _eventSource.KeyUp -= OnKeyUp;
            }

            _eventSource       =  window;
            _eventSource.KeyUp += OnKeyDown;
            _eventSource.KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _keys.OnNext(new WindowKeyEventArgs
            {
                Args = e,
                IsDown = true,
            });
        }
        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _keys.OnNext(new WindowKeyEventArgs
            {
                Args   = e,
                IsDown = false,
            });
        }
        
        public IObservable<WindowKeyEventArgs> Keys => _keys;
    }
}