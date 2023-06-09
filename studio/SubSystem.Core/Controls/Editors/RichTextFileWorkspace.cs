using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public class RichTextFileWorkspace : Workspace
    {
        public PartOfRtf Part { get; init; }
        public RichTextBox Control { get; set; }
    }
}