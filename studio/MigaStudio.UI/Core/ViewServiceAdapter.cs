using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class ViewServiceAdapter : IViewServiceAdapter, IViewServiceAmbient
    {
        private readonly ViewService _service;
        
        public ViewServiceAdapter()
        {
            _service = new ViewService();
        }
        

        public void Route<TViewModel>(NavigationParameter parameter) where TViewModel : IViewModel
        {
            var viewModel = Xaml.GetViewModel<TViewModel>();
            
            if (parameter.Controller is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _service.Route(viewModel, parameter.Params);
        }
        

        public void Route(IViewModel viewModel, NavigationParameter parameter)
        {
            if (parameter.Controller is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _service.Route(viewModel, parameter.Params);
        }
        
        public void SetServiceProvider(ViewHostBase host) => _service.SetServiceProvider(host);

        public IViewService Host => _service;
        
        public ITabViewController Controller { get; set; }
    }
}