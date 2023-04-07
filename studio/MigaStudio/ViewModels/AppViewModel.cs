using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Services;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBaseAppViewModel
    {
        private readonly GlobalStudioContext _context;
        private readonly RouteEventArgs           _parameter;

        public AppViewModel()
        {
            var shell  = new TabShell();
            var launch = new LaunchViewController();
            var quick  = new QuickStartController();

            _context = new GlobalStudioContext
            {
                MainController = shell,
                Controllers = new  ITabViewController[]
                {
                    shell,
                    launch,
                    quick
                },
                ControllerMaps = new Dictionary<string, ITabViewController>
                {
                    { shell.Id, shell },
                    { launch.Id, launch },
                    { quick.Id, quick },
                },
                ControllerSetter = x => CurrentController = x
            };
            
            _parameter = new RouteEventArgs
            {
                Args = new object[]
                {
                    _context
                }
            };
            OnInitialize(_context);
            Controller        = shell;
        }

        protected override bool OnKeyDown(WindowKeyEventArgs e)
        {
            if (e.Args.Key == Key.F5)
            {
                Xaml.Get<MusicService>()
                    .PlayOrPause();
                return true;
            }
            return base.OnKeyDown(e);
        }

        protected sealed override void OnControllerChanged(IViewController oldController, IViewController newController)
        {
            if (newController is null)
            {
                return;
            }
            
            newController.Startup(_parameter);
            newController.Start();
        }

        protected override void StartOverride()
        {
            CurrentController = _context.Controllers.First(x => x is LaunchViewController);
        }
    }
}