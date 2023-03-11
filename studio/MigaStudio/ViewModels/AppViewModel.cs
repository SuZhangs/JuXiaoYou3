using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Views;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBaseAppViewModel
    {
        private readonly GlobalStudioContext _context;
        private readonly Parameter           _parameter;

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
            
            _parameter = new Parameter
            {
                Args = new object[]
                {
                    _context
                }
            };
            OnInitialize(_context);
            Controller        = shell;
        }

        protected sealed override void OnControllerChanged(IViewController oldController, IViewController newController)
        {
            if (newController is null)
            {
                return;
            }
            
            newController.Start(_parameter);
            newController.Start();
        }

        protected override void StartOverride()
        {
            CurrentController = _context.Controllers.First(x => x is LaunchViewController);
        }
    }
}