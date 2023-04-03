using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageViewModel : DialogViewModel
    {
        protected override void OnStart(RouteEventArgs parameter)
        {
            var fileName = parameter.Args[0]?.ToString();
            base.OnStart(parameter);
        }
    }
}