using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Editors.Completion;
using Acorisoft.FutureGL.MigaStudio.Editors.Models;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public class MarkdownWorkspace : Workspace<TextEditor>
    {
        private ConceptCompletionWindow _window;
        private bool                    _isShow;
        
        
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
            if (!string.IsNullOrEmpty(Part?.Content))
            {
                Control.Text = Part.Content;
            }


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

        public override void Inactive()
        {
            Control.Visibility = Visibility.Collapsed;
        }

        public override void Active()
        {
            Control.Visibility = Visibility.Visible;
            UpdateDocumentState();
        }

        private void UpdateDocumentState()
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
        }

        private void OnWindowClose(object sender, EventArgs e)
        {
            _window.Closed -= OnWindowClose;
            _window        =  null;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (_window is null)
            {
                _isShow        =  false;
                _window        =  new ConceptCompletionWindow(Control.TextArea);
                _window.Closed += OnWindowClose;
            }

            var data = _window.CompletionList
                              .CompletionData;
            var engine = Studio.Engine<DocumentEngine>();
            var concepts = engine.GetConcepts()
                                 .Select(x => new ConceptCompletionData
                                 {
                                     Text    = x.Name,
                                     Content = x.Name
                                 });
            data.AddMany(concepts, true);

            Part.Content = Control.Text;
            WorkspaceChanged?.Invoke(StateChangedEventSource.TextSource, this);

            //
            //
            if (_isShow)
            {
                _window.Close();
            }

            if (_window is null)
            {
                _isShow = false;
                return;
            }

            _isShow = true;
            _window.Show();
        }

        public sealed override void UpdateCaretState()
        {
            if (Control is null)
            {
                LineNumber = -1;
                LineColumn = -1;
               return;
            }

            var caret = Control.TextArea
                               .Caret;

            LineNumber = caret.Line;
            LineColumn = caret.Column;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Content => Part is null ? string.Empty : Part.Content;
        
        /// <summary>
        /// 
        /// </summary>
        public PartOfMarkdown Part { get; init; }
    }
}