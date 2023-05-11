using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class UniverseEditorViewModel
    {
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
        }
        
        private static void InitializeSpace(SpaceConceptOverview space)
        {
            
        }
        
        private static void InitializeDocument(PropertyOverview property)
        {
            InitializeProperty(property, "", BrowsablePropertyDataSource.NPC);
        }

        private static void InitializeProperty(PropertyOverview property, string uid, BrowsablePropertyDataSource dataSource)
        {
            property.Add(new BrowsableProperty
            {
                Uid = uid,
                Source = dataSource,
            });
        }
        
        private static void InitializeRule(PropertyOverview rule)
        {
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
        
        


        /// <summary>
        /// 获取或设置 <see cref="SelectedView"/> 属性。
        /// </summary>
        public FrameworkElement SelectedView
        {
            get => _selectedView;
            private set => SetValue(ref _selectedView, value);
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

                SelectedView = ServiceViewContainer.GetBrowsableView(this, _selectedBrowsableElement);
            }
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        public ObservableCollection<IBrowsableRoot> BrowsableElements { get; }
    }
}