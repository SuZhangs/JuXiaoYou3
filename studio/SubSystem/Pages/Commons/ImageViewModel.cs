using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageViewModel : DialogViewModel
    {
        protected override void OnStart(RoutingEventArgs parameter)
        {
            var fileName = parameter.Parameter
                                    .Args[0]
                                    ?.ToString();
            base.OnStart(parameter);
        }
    }
}