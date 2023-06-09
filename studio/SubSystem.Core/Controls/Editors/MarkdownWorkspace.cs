using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Acorisoft.FutureGL.MigaStudio.Controls.Editors.Models;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

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
                      .Subscribe(x => { OnTextChanged(x.Sender, x.EventArgs); })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea.SelectionChanged   += added,
                          removed => Control.TextArea.SelectionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x => { OnSelectionChanged(x.Sender, x.EventArgs); })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea
                                          .Caret
                                          .PositionChanged += added,
                          removed => Control.TextArea
                                            .Caret
                                            .PositionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x => { OnPositionChanged(x.Sender, x.EventArgs); })
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
            var window = new ConceptCompletionWindow(Control.TextArea);
            var data   = window.CompletionList
                               .CompletionData;
            var engine   = Studio.Engine<DocumentEngine>();
            var concepts = engine.GetConcepts()
                                 .Select(x => new ConceptCompletionData
                                 {
                                     Text = x.Name,
                                 });
            data.AddMany(concepts, true);
            window.Show();
            Part.Content = Control.Text;
        }

        public override string Content => Part is null ? string.Empty : Part.Content;
        public PartOfMarkdown Part { get; init; }
        public TextEditor Control { get; set; }
    }
}