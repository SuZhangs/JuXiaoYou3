using System;
using System.Windows.Controls;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaUI;
using Acorisoft.FutureGL.MigaUI.Contracts;
using Acorisoft.FutureGL.MigaUI.ViewModels;
using Acorisoft.FutureGL.MigaUI.Views;

namespace Acorisoft.FutureGL.MigaStudio.ToolKits.TemplateSystem
{
    public partial class AView : DialogPage
    {
        public AView()
        {
            InitializeComponent();
        }
    }

    public sealed class ADialogViewModel : OperationViewModel
    {
    }
}