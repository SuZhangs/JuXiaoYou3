using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class UniverseEditorViewModel
    {
        #region Initialize

        
        private void InitializeBrowsableElements()
        {
            var universe     = new UniversalIntroduction();
            var space        = new SpaceConceptOverview();
            var documentRoot = new PropertyOverview("text.Universe.Overview.Document");
            var ruleRoot     = new PropertyOverview("text.Universe.Overview.Rule");
            var other        = new OtherIntroduction();
            
            //
            // 初始化
            InitializeUniverse(universe);
            InitializeSpace(space);
            InitializeDocument(documentRoot);
            InitializeRule(ruleRoot);
            InitializeOther(other);
            
            //
            // 添加
            BrowsableElements.Add(universe);
            BrowsableElements.Add(space);
            BrowsableElements.Add(documentRoot);
            BrowsableElements.Add(ruleRoot);
            BrowsableElements.Add(other);
            
            //
            // 准备文本
            InitializeTextSource();
        }

        private static void InitializeSpaceConcept(SpaceConcept concept)
        {
            //
            // 获得子空间概念
            
            //
            // 添加属性
            concept.Add(new SpaceProperty
            {
                Uid = ""
            });
        }

        private static void InitializeUniverse(UniversalIntroduction universe)
        {
            universe.Add(new RarityConcept());
            universe.Add(new DeclarationConcept());
        }
        
        private static void InitializeSpace(SpaceConceptOverview space)
        {
            
        }
        
        private static void InitializeDocument(PropertyOverview property)
        {
            InitializeProperty(property,  BrowsablePropertyDataSource.Country);
            InitializeProperty(property,  BrowsablePropertyDataSource.Gangbang);
            InitializeProperty(property,  BrowsablePropertyDataSource.Team);
            InitializeProperty(property,  BrowsablePropertyDataSource.Planets);
            InitializeProperty(property,  BrowsablePropertyDataSource.Creatures);
            InitializeProperty(property,  BrowsablePropertyDataSource.Material);
            InitializeProperty(property,  BrowsablePropertyDataSource.Ore);
            InitializeProperty(property,  BrowsablePropertyDataSource.NPC);
        }

        private static void InitializeProperty(PropertyOverview property, BrowsablePropertyDataSource dataSource)
        {
            property.Add(new BrowsableProperty
            {
                Name = Language.GetEnum(dataSource),
                Source = dataSource,
            });
        }
        
        private static void InitializeRule(PropertyOverview rule)
        {
            InitializeProperty(rule,  BrowsablePropertyDataSource.Gods);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Devils);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Technology);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Elemental);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Poison);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Calamity);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Magic);
            InitializeProperty(rule,  BrowsablePropertyDataSource.Physic);
        }
        
        private static void InitializeOther(Introduction universe)
        {
        }

        private void InitializeTextSource()
        {
            foreach (var element in BrowsableElements)
            {
                InitializeTextSource(element);
                
                foreach (var child in element.Children)
                {
                   InitializeTextSource(child);
                }
            }
        }

        private static void InitializeTextSource(IBrowsable browsable)
        {
            if (!string.IsNullOrEmpty(browsable.Uid))
            {
                browsable.Name = Language.GetText(browsable.Uid);
            }
            
            if (browsable is IBrowsableRoot root)
            {
                foreach (var child in root.Children)
                {
                    InitializeTextSource(child);
                }
            }
        }
        
        

        #endregion


        /// <summary>
        /// 获取或设置 <see cref="SelectedView"/> 属性。
        /// </summary>
        public FrameworkElement SelectedView
        {
            get => _selectedView;
            private set
            {
                if (_selectedView is not null)
                {
                    var inspectable = (InspectableViewModel)_selectedView.DataContext;
                    inspectable?.Stop();
                }

                SetValue(ref _selectedView, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedBrowsableElement"/> 属性。
        /// </summary>
        public IBrowsable SelectedBrowsableElement
        {
            get => _selectedBrowsableElement;
            set
            {
                SetValue(ref _selectedBrowsableElement, value);

                if (_selectedBrowsableElement is null)
                {
                    return;
                }

                SelectedView = ServiceViewContainer.Build(this, _selectedBrowsableElement);
            }
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        public ObservableCollection<IBrowsableRoot> BrowsableElements { get; }
    }
}