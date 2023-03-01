using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class ViewService : IViewService, IViewServiceAmbient
    {
        private ViewHostBase _viewHost;
        public void Route<TViewModel>()  where TViewModel : IViewModel
        {
            Route<TViewModel>(new Parameter());
        }

        public void Route<TViewModel>(Parameter parameter) where TViewModel : IViewModel
        {
            var viewModel = Xaml.GetViewModel<TViewModel>();
            Route(viewModel, new Parameter());
        }

        public void Route(IViewModel viewModel)
        {
            Route(viewModel, new Parameter());
        }

        public void Route(IViewModel viewModel, Parameter parameter)
        {
            viewModel.Start(parameter);
            _viewHost.ViewModel = viewModel;
            
        }

        public void SetServiceProvider(ViewHostBase host) => _viewHost = host;
    }
}