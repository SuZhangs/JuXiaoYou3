﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
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
            var shell       = new TabShell();
            var launch      = new LaunchViewController();
            var quick       = new QuickStartController();
            var interaction = new InteractionController();

            _context = new GlobalStudioContext
            {
                MainController = shell,
                Controllers = new List<ITabViewController>
                {
                    shell,
                    launch,
                    quick,
                    interaction
                },
                Character             = null,
                FavoriteCharacterList = new ObservableCollection<DocumentCache>(),
                ControllerList = new ObservableCollection<NamedItem<string>>
                {
                    new NamedItem<string>
                    {
                        Name  = Language.GetText(IdOfTabShellController),
                        Value = IdOfTabShellController
                    },
                    new NamedItem<string>
                    {
                        Name  = Language.GetText(IdOfInteractionController),
                        Value = IdOfInteractionController
                    },
                },
                ControllerMaps = new Dictionary<string, ITabViewController>
                {
                    { shell.Id, shell },
                    { interaction.Id, interaction },
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

            InitializeBuiltinController(_context);
            OnInitialize(_context);
            Controller = shell;
        }

        private static void InitializeBuiltinController(GlobalStudioContext context)
        {
            InitializeController(context, () => new WorldViewController());
        }

        private static void InitializeController(GlobalStudioContext context, Func<TabController> factory)
        {
            var controller = factory();
            var id         = controller.Id;
            var item = new NamedItem<string>
            {
                Name  = Language.GetText(id),
                Value = id,
            };
            
            context.ControllerList.Add(item);
            ((List<ITabViewController>)context.Controllers).Add(controller);
            ((Dictionary<string, ITabViewController>)context.ControllerMaps).TryAdd(id, controller);

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
                    Xaml.Get<IDialogService>()
                        .Dialog(new MusicPlayerViewModel());
                    return true;
                case Key.F6:
                    return true;
                case Key.F7:
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
        public const string IdOfWorldViewController   = "__WorldView";
        public const string IdOfLaunchController      = "__Launch";
        public const string IdOfInteractionController = "__Interaction";
        public const string IdOfVisitorController     = "__Visitor";
        public const string IdOfTabShellController    = "__Main";
    }
}