using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Services;
using Acorisoft.FutureGL.MigaUtils;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBaseAppViewModel
    {
        private readonly GlobalStudioContext _context;
        private readonly RoutingEventArgs    _parameter;

        public AppViewModel()
        {
            var shell  = new TabShell();
            var launch = new LaunchViewController();
            var quick  = new QuickStartController();

            _context = new GlobalStudioContext
            {
                MainController = shell,
                Controllers = new ITabViewController[]
                {
                    shell,
                    launch,
                    quick
                },
                Character = null,
                FavoriteCharacterList = new ObservableCollection<DocumentCache>(),
                ControllerList = new ObservableCollection<NamedItem<string>>
                {
                    new NamedItem<string>
                    {
                        Name = Language.GetText(IdOfTabShellController),
                        Value = shell.Id
                    },
                },
                ControllerMaps = new Dictionary<string, ITabViewController>
                {
                    { shell.Id, shell },
                    { launch.Id, launch },
                    { quick.Id, quick },
                },
                ControllerSetter = x => CurrentController = x,
            };

            _parameter = new RoutingEventArgs
            {
                Parameter = new Parameter
                {
                    Args = new object[]
                    {
                        _context
                    }
                }
            };
            OnInitialize(_context);
            Controller = shell;
        }

        protected override bool OnKeyDown(WindowKeyEventArgs e)
        {
            var key = e.Args.Key;

            switch (key)
            {
                case Key.F1:
                    ((TabController)Controller).New<SettingViewModel>();
                    return true;
                case Key.F5:
                    Xaml.Get<MusicService>()
                        .PlayOrPause();
                    return true;
                case Key.F6:
                    Xaml.Get<MusicService>()
                        .PlayLast();
                    return true;
                case Key.F7:
                    Xaml.Get<MusicService>()
                        .PlayNext();
                    return true;
                case Key.F12:
                    SwitchModeImpl();
                    return true;
                default:
                    break;
            }

            return base.OnKeyDown(e);
        }

        private void SwitchModeImpl()
        {
            var ccl    = _context.ControllerList;
            var ccl_fv = ccl.FirstOrDefault();
            if (ccl.Count > 1 || 
                (ccl.Count == 1 && 
                 ccl_fv is not null && 
                 ccl_fv.Value != IdOfTabShellController))
            {
                
                ModeSwitchViewModel.Switch(_context);
            }
        }

        protected sealed override void OnControllerChanged(IViewController oldController, IViewController newController)
        {
            if (newController is null)
            {
                return;
            }

            if (newController.IsInitialized)
            {
                newController.Resume();
            }
            else
            {
                newController.Startup(_parameter);
                newController.Start();
            }
        }

        protected override void StartOverride()
        {
            CurrentController = _context.Controllers.First(x => x is LaunchViewController);
        }

        public const string IdOfQuickStartController  = "__Quick";
        public const string IdOfStoryboardController  = "__Storyboard";
        public const string IdOfLaunchController      = "__Launch";
        public const string IdOfInspirationController = "__Inspiration";
        public const string IdOfVisitorController     = "__Visitor";
        public const string IdOfTabShellController    = "__Main";
    }
}