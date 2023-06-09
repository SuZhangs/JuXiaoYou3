using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ICSharpCode.AvalonEdit;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class MarkdownWorkspace : Workspace
    {
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
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnTextChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea.SelectionChanged   += added,
                          removed => Control.TextArea.SelectionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnSelectionChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea
                                          .Caret
                                          .PositionChanged   += added,
                          removed => Control.TextArea
                                            .Caret
                                            .PositionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnPositionChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
        }
        
        private void OnPositionChanged(object sender, EventArgs e)
        {
            
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            Part.Content = Control.Text;
        }

        public override string Content
        {
            get
            {
                if (Part is null)
                {
                    return string.Empty;
                }

                return Part.Content;
            }
        }

        public PartOfMarkdown Part { get; init; }
        public TextEditor Control { get; set; }
    }
}