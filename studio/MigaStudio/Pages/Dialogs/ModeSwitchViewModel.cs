using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ModeSwitchViewModel : ImplicitDialogVM
    {
        private string _selectedController;

        public ModeSwitchViewModel()
        {
            Controllers = new ObservableCollection<NamedItem<string>>
            {
                new NamedItem<string>
                {
                    Name = Language.GetText(AppViewModel.IdOfTabShellController),
                    Value = AppViewModel.IdOfTabShellController
                },
                new NamedItem<string>
                {
                    Name  = Language.GetText(AppViewModel.IdOfStoryboardController),
                    Value = AppViewModel.IdOfStoryboardController
                },
                new NamedItem<string>
                {
                    Name  = Language.GetText(AppViewModel.IdOfVisitorController),
                    Value = AppViewModel.IdOfVisitorController
                },
                new NamedItem<string>
                {
                    Name  = Language.GetText(AppViewModel.IdOfInspirationController),
                    Value = AppViewModel.IdOfInspirationController
                },
            };
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            Context            = (GlobalStudioContext)a[0];
            SelectedController = Context.CurrentController.Id;
            base.OnStart(parameter);
        }

        public static Task<Op<object>> Switch(GlobalStudioContext context)
        {
            return DialogService().Dialog(new ModeSwitchViewModel(), new Parameter
            {
                Args = new object[] { context }
            });
        }

        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            if (string.IsNullOrEmpty(SelectedController))
            {
                return;
            }

            if (!Context.ControllerMaps
                        .TryGetValue(SelectedController, out var controller))
            {
                return;
            }
            
            Context.SwitchController(controller);
        }

        protected override string Failed() => SubSystemString.Unknown;

        public GlobalStudioContext Context { get; private set; }

        public ObservableCollection<NamedItem<string>> Controllers { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedController"/> 属性。
        /// </summary>
        public string SelectedController
        {
            get => _selectedController;
            set => SetValue(ref _selectedController, value);
        }
    }
}