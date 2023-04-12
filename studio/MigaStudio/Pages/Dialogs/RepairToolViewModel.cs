using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RepairToolViewModel : DialogViewModel
    {
        public Task KillProcessImpl()
        {
            return Task.Run(ProcessUtilities.Kill);
        }
    }
}