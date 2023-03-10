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
        

        public void Route<TViewModel>(NavigationParameter parameter) where TViewModel : ITabViewModel
        {
            var viewModel = Xaml.GetViewModel<TViewModel>();
            
            if (parameter.Controller is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Route(viewModel, null);
        }
        

        public void Route(ITabViewModel viewModel, NavigationParameter parameter)
        {
            if (parameter.Controller is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            viewModel.Start(parameter.Params ?? NavigationParameter.New(viewModel, Controller).Params);
            Controller.Start(viewModel);
        }
        
        public void SetServiceProvider(ViewHostBase host) => _service.SetServiceProvider(host);

        public IViewService Host => _service;
        
        public ITabViewController Controller { get; set; }
    }
}