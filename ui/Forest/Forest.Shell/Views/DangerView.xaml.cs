using System.Windows.Controls;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class DangerView:ForestUserControl 
    {
        public DangerView()
        {
            InitializeComponent();
        }
    }

    public sealed class DangerViewModel : OperationViewModel
    {
        private string _content;
        private int _tick;
        private DispatcherTimer _timer;
        private string _text;

        protected override bool IsCompleted() => !CountDown || (CountDown && _tick <= 0);

        internal override void StartOverride()
        {
            if (CountDown)
            {
                _text = CompleteButtonText;
                _tick = Math.Clamp(CountSeconds, 5, 60);
                _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (_, _) =>
                {
                    _tick--;


                    UpdateOkText();

                    if (_tick <= 0)
                    {
                        _timer.Stop();
                    }
                }, Dispatcher.CurrentDispatcher);

                UpdateOkText();
                _timer.Start();
            }

            base.StartOverride();
        }

        private void UpdateOkText()
        {
            CompleteButtonText = _tick > 0 ? $"{_text} ({_tick})" : _text;
            CompletedCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public int CountSeconds { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="CountDown"/> 属性。
        /// </summary>
        public bool CountDown { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}