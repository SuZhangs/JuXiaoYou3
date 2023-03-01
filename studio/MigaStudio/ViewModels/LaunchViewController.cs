using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewController : ViewModelBase, ISplashController
    {
        public LaunchViewController(TabBased globalParameter)
        {
            AppViewModel = globalParameter;
        }

        public override void Start()
        {
        }

        public TabBased AppViewModel { get; }
    }
}