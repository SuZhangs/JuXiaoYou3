using ICSharpCode.AvalonEdit;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class MarkdownWorkspace : Workspace
    {
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