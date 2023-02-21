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
        
        IObservable<WindowDragDropArgs> Drags { get; }
    }

    public interface IWindowEventBroadcastAmbient
    {
        void SetEventSource(Window window);
    }



    public class WindowEventBroadcast : IWindowEventBroadcast, IWindowEventBroadcastAmbient
    {
        private readonly Subject<WindowKeyEventArgs> _keys  = new Subject<WindowKeyEventArgs>();
        private readonly Subject<WindowDragDropArgs> _drags = new Subject<WindowDragDropArgs>();

        private Window _eventSource;

        public void SetEventSource(Window window)
        {
            if (window is null)
            {
                return;
            }
            
            if (_eventSource is not null)
            {
                _eventSource.KeyUp     -= OnKeyDown;
                _eventSource.KeyUp     -= OnKeyUp;
                _eventSource.DragEnter -= OnDragEnter;
                _eventSource.DragOver  -= OnDragOver;
                _eventSource.DragLeave -= OnDragLeave;
            }

            _eventSource       =  window;
            _eventSource.KeyUp += OnKeyDown;
            _eventSource.KeyUp += OnKeyUp;
            _eventSource.DragEnter += OnDragEnter;
            _eventSource.DragOver += OnDragOver;
            _eventSource.DragLeave += OnDragLeave;
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            _drags.OnNext(new WindowDragDropArgs
            {
                State = DragDropState.Dropped,
                Args = e
            });
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            _drags.OnNext(new WindowDragDropArgs
            {
                State = DragDropState.Dragging,
                Args  = e
            });
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            _drags.OnNext(new WindowDragDropArgs
            {
                State = DragDropState.DragStart,
                Args  = e
            });
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
        public IObservable<WindowDragDropArgs> Drags => _drags;
    }
}