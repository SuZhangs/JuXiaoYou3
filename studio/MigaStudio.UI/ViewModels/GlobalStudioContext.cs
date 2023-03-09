using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.AppModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class GlobalStudioContext 
    {
        public void SwitchController(ITabViewController controller)
        {
            if(controller is null) return;
            ControllerSetter?.Invoke(controller);
        }
        
        public Action<ITabViewController> ControllerSetter { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ITabViewController MainController { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<ITabViewController> Controllers { get; init; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, ITabViewController> ControllerMaps { get; init; }
    }
}