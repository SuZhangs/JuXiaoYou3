using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class ViewService : IViewService, IViewServiceAmbient
    {
        private ViewHostBase _viewHost;
        public void Route<TViewModel>()  where TViewModel : IViewModel
        {
            Route<TViewModel>(new RouteEventArgs());
        }

        public void Route<TViewModel>(RouteEventArgs parameter) where TViewModel : IViewModel
        {
            var viewModel = Xaml.GetViewModel<TViewModel>();
            Route(viewModel, new RouteEventArgs());
        }

        public void Route(IViewModel viewModel)
        {
            Route(viewModel, new RouteEventArgs { Args = new object[8] });
        }

        public void Route(IViewModel viewModel, RouteEventArgs parameter)
        {
            viewModel.Startup(parameter);
            _viewHost.ViewModel = viewModel;
            
        }

        public void SetServiceProvider(ViewHostBase host) => _viewHost = host;
    }
}