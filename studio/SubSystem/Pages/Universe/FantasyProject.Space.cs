using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class FantasyProjectSpaceConceptViewModel : TabViewModel
    {
        protected override void OnStart(Parameter parameter)
        {
            var a    = parameter.Args;
            Root =   a[0] as ProjectItem;
            Root ??= new ProjectItem();
            base.OnStart(parameter);
        }
        
        public ProjectItem Root { get; private set; }
    }
}