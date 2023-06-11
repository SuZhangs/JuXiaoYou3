using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public class RichTextFileWorkspace : Workspace<RichTextBox>
    {
        
        public PartOfRtf Part { get; init; }

        public override bool CanUndo() => Control.CanUndo;

        public override bool CanRedo()=> Control.CanRedo;

        public override void Undo()
        {
            if (Control is null)
            {
                return;
            }

            if (!CanUndo())
            {
                return;
            }

            Control.Undo();
        }

        public override void Redo()
        {
            if (Control is null)
            {
                return;
            }

            if (!CanRedo())
            {
                return;
            }

            Control.Redo();
        }

        public override void Initialize()
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