using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewController : LaunchViewControllerBase
    {

        public LaunchViewController()
        {
            // 加载设置
            Job(SubSystemString.GetText("text.launch.loadSetting"), _ =>
            {
                
            });
            
            // 检查更新
            Job(SubSystemString.GetText("text.launch.checkVersion"), x =>
            {
            });
            
            Job(SubSystemString.GetText("text.launch.openDatabase"), x => OpenDatabaseImpl((GlobalStudioContext)x));
        }

        private void OpenDatabaseImpl(GlobalStudioContext context)
        {
            var setting = Xaml.Get<SystemSetting>()
                              .RepositorySetting;

            if (string.IsNullOrEmpty(setting.LastRepository))
            {
                return;
            }

            var dr = Xaml.Get<IDatabaseManager>()
                         .LoadAsync(setting.LastRepository)
                         .GetAwaiter()
                         .GetResult();
            
            if (dr.IsFinished)
            {
                context.IsDatabaseOpen = true;
            }
            else
            {
                Xaml.Get<IDialogService>()
                    .Notify(
                        CriticalLevel.Warning,
                        Language.NotifyText,
                        SubSystemString.GetDatabaseResult(dr.Reason));
            }
        }

        protected override void OnStartup(RoutingEventArgs arg)
        {
            Context = arg.Parameter.Args[0] as GlobalStudioContext;
            Init();
        }

        protected override object GetExecuteContext() => Context;

        protected override void OnJobCompleted()
        {
            var opening    = Context.IsDatabaseOpen;
            var controller = opening ? 
                Context.Controllers.First(x => x is TabShell) : 
                Context.Controllers.First(x => x is QuickStartController) ;
            
            //
            //
            Context.SwitchController(controller);
        }
        
        public GlobalStudioContext Context { get; private set; }
    }
}