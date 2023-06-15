using System.Diagnostics;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class FantasyProjectSpaceConceptViewModel : TabViewModel
    {
        public FantasyProjectSpaceConceptViewModel()
        {
            Children   = new ObservableCollection<SpaceConceptUI>();
            
            AddCommand = AsyncCommand<SpaceConceptUI>(AddImpl);
        }
        
        protected override void OnStart(Parameter parameter)
        {
            var a    = parameter.Args;
            Root   =   a[0] as ProjectItem;
            Handle =   a[2] as SpaceConceptUI;
            Parent =   a[3] as SpaceConceptUI;
            Root   ??= new ProjectItem();
            base.OnStart(parameter);
        }


        private async Task AddImpl(SpaceConceptUI parent)
        {
            var r = await SingleLineViewModel.String("Add");

            if (!r.IsFinished)
            {
                return;
            }
            
            #if DEBUG
            Debug.WriteLine($"handle: {Handle.Name}, parent: {Parent.Name}");
            #endif
            
            if (parent is null)
            {
                var spaceConcept = new SpaceConcept
                {
                    Id     = ID.Get(),
                    Name   = r.Value,
                    Height = 0
                };

                var ui = new SpaceConceptUI
                {
                    Source   = spaceConcept,
                    Children = new ObservableCollection<SpaceConceptUI>(),
                };

                ui.Item = new ProjectItem
                {
                    Name          = spaceConcept.Name,
                    Children      = new ObservableCollection<ProjectItem>(),
                    Parameter1    = ui,
                    Parameter2    = Root,
                    Expression    = FantasyProjectStartupViewModel.CreateSpaceConceptExpr,
                    ViewModelType = typeof(FantasyProjectSpaceConceptViewModel),

                };
                
                Children.Add(ui);
                Root.Add(ui.Item);
            }
            else
            {
                if (parent.Source.Height > 16)
                {
                    // 宇宙 星系 星球 大陆 国家 省份 城市 区县 乡镇 村 屯
                    await this.WarningNotification("不支持超过16级的空间");
                    return;
                }
                
                var spaceConcept = new SpaceConcept
                {
                    Id     = ID.Get(),
                    Name   = r.Value,
                    Height = parent.Source.Height + 1,
                    Owner = parent.Source.Id,
                };

                var ui = new SpaceConceptUI
                {
                    Parent = parent,
                    Source   = spaceConcept,
                    Children = new ObservableCollection<SpaceConceptUI>(),
                };

                ui.Item = new ProjectItem
                {
                    Name          = spaceConcept.Name,
                    Children      = new ObservableCollection<ProjectItem>(),
                    Parameter1    = ui,
                    Parameter2    = Root,
                    Parameter3    = parent,
                    Expression    = FantasyProjectStartupViewModel.CreateSpaceConceptExpr,
                    ViewModelType = typeof(FantasyProjectSpaceConceptViewModel),
                };
                
                parent.Children
                      .Add(ui);
                Children.Add(ui);
                Root.Add(ui.Item);
            }
        }
        
        public AsyncRelayCommand<SpaceConceptUI> AddCommand { get; }
        public AsyncRelayCommand<SpaceConceptUI> EditCommand { get; }
        public AsyncRelayCommand<SpaceConceptUI> RemoveCommand { get; }
        
        public ProjectItem Root { get; private set; }
        
        /// <summary>
        /// 当前的UI 对象
        /// </summary>
        public SpaceConceptUI Handle { get; private set; }
        public SpaceConceptUI Parent { get; private set; }
        public ObservableCollection<SpaceConceptUI> Children { get; }
    }
}