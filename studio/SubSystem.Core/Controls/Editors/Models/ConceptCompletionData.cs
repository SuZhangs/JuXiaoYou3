﻿using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors.Models
{
    public class ConceptCompletionData : ICompletionData
    {
        // https://www.cnblogs.com/nankezhishi/archive/2008/11/22/1338959.html
        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            throw new NotImplementedException();
        }

        public ImageSource Image { get;init; }
        public string Text { get;init; }
        public object Content { get;init; }
        public object Description { get;init; }
        public double Priority { get; init; }
    }
}