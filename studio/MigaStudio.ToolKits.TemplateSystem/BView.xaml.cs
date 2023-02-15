using System.Windows.Controls;
using Acorisoft.FutureGL.MigaUI.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ToolKits.TemplateSystem
{
    public partial class BView : UserControl
    {
        public BView()
        {
            InitializeComponent();
        }
    }
    
    public sealed class BDialogViewModel  : InputViewModel
    {
        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            Result = GetHashCode();
        }

        protected override string Failed()
        {
            return $"{GetHashCode()} : Failed";
        }
    }
}