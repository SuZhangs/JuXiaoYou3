﻿using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Acorisoft.FutureGL.MigaStudio.Controls.Editors.Models;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class MarkdownWorkspace : Workspace
    {
        private ConceptCompletionWindow _window;
        private bool                    _isShow;
        
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
            
            var data   = _window.CompletionList
                                .CompletionData;
            var engine   = Studio.Engine<DocumentEngine>();
            var concepts = engine.GetConcepts()
                                 .Select(x => new ConceptCompletionData
                                 {
                                     Text = x.Name,
                                     Content = x.Name
                                 });
            data.AddMany(concepts, true);
            
            //
            //
            if (_isShow)
            {
                _window.Close();
            }
            
            _isShow = true;
            _window.Show();
            Part.Content = Control.Text;
        }

        public override string Content => Part is null ? string.Empty : Part.Content;
        public PartOfMarkdown Part { get; init; }
        public TextEditor Control { get; set; }
    }
}