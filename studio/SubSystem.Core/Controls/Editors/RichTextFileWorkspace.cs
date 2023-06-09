using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class RichTextFileWorkspace : Workspace
    {
        public PartOfRtf Part { get; init; }
        public RichTextBox Control { get; set; }
        public override void Immutable()
        {
            if (IsWorking)
            {
                return;
            }

            IsWorking = true;
            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                          added => Control.TextChanged   += added,
                          removed => Control.TextChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnTextChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                          added => Control.SelectionChanged   += added,
                          removed => Control.SelectionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnSelectionChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
            
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.TextSource, this);
        }

        public override string Content =>  Part is null ? string.Empty : Part.Content;
    }
}