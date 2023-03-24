using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class HomeViewModel : TabViewModel
    {
        public HomeViewModel()
        {
            Title = "首页";

            GotoTemplateEditorCommand = Command(GotoTemplateEditorImpl);
        }

        private void GotoTemplateEditorImpl()
        {
            Controller.New<TemplateEditorViewModel>();
        }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand GotoTemplateEditorCommand { get; }
    }
}