using System.Collections.Generic;
using System.Threading;
using Acorisoft.FutureGL.MigaUI.Models;
using Acorisoft.FutureGL.MigaUI.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ToolKits.ViewModels
{
    public class AppViewModel : AppViewModelBase
    {
        protected override IReadOnlyList<WindowInitialization> PrepareInitializations()
        {
            return new WindowInitialization[]
            {
                new WindowInitialization
                {
                    Name = "加载设置...",
                    Work = _ => {},
                },
                new WindowInitialization
                {
                    Name = "准备数据库...",
                    Work = _ => {},
                },
                new WindowInitialization
                {
                    Name = "等待2s",
                    Work = _ => {},
                },
            };
        }
    }
}