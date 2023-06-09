using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class RichTextFileWorkspace : Workspace
    {
        private bool _immutable;
        
        public PartOfRtf Part { get; init; }
        public RichTextBox Control { get; set; }
        public override void Immutable()
        {
            if (IsWorking)
            {
                return;
            }

            IsWorking = true;
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextChanged   += added,
                          removed => Control.TextChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(_ =>
                      {
                      });
            
            Control.TextChanged += OnTextChanged;
            Control.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        public override string Content { get; }
    }
}