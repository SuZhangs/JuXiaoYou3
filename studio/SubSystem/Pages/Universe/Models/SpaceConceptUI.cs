using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe.Models
{
    public class SpaceConceptUI : ObservableObject
    {
        private SpaceConceptUI _parent;

        public SpaceConcept Source { get; init; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Intro
        {
            get => Source.Intro;
            set
            {
                Source.Intro = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 父级
        /// </summary>
        public SpaceConceptUI Parent
        {
            get => _parent;
            set => SetValue(ref _parent, value);
        }

        /// <summary>
        /// 子级
        /// </summary>
        public ObservableCollection<SpaceConceptUI> Children { get; init; }
    }
}