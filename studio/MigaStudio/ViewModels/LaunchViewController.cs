using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Models;
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
            
            Job(SubSystemString.GetText("text.launch.openDatabase"), x =>
            {
                var dm = Xaml.Get<IDatabaseManager>();
                
                var dr = Xaml.Get<IDatabaseManager>()
                             .LoadAsync(@"C:\Users\Administrator\Documents\我的世界观\Juxiaoyou3")
                             .GetAwaiter()
                             .GetResult();
                if (dr.IsFinished)
                {
                    ((GlobalStudioContext)x).IsDatabaseOpen = true;
                }
            });
        }

        protected override void OnStartup(Parameter arg)
        {
            Context = arg.Args[0] as GlobalStudioContext;
            Init();
        }

        protected override object GetExecuteContext() => Context;

        protected override void OnJobCompleted()
        {
            var opening    = Context.IsDatabaseOpen;
            var controller = opening ? Context.Controllers.First(x => x is TabShell) : Context.Controllers.First(x => x is QuickStartController) ;
            
            //
            //
            Context.SwitchController(controller);
        }
        
        public GlobalStudioContext Context { get; private set; }
    }
}