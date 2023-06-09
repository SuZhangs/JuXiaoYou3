using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Editors
{
    public static class MarkdownUtilities
    {
        
        static void Test()
        {

            var rtf = new RichTextBox();
            rtf.SelectionChanged += OnSelectionChanged;
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}