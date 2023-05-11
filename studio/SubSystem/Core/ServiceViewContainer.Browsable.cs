using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using NLog.Targets.Wrappers;
using UniverseEditorViewModel = Acorisoft.FutureGL.MigaStudio.Pages.Universe.UniverseEditorViewModel;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    using BrowsableDictionary = Dictionary<Type, ViewFactory<UniverseEditorViewModel>>;

    partial class ServiceViewContainer
    {
        private static readonly BrowsableDictionary BrowsableMappings = new BrowsableDictionary();

        public static FrameworkElement GetBrowsableView(UniverseEditorViewModel owner, IBrowsable browsable)
        {
            if (browsable is null)
            {
                return null;
            }

            if (BrowsableMappings.TryGetValue(browsable.GetType(), out var mapping))
            {
                return mapping(owner, browsable);
            }

            return new UserControl
            {
                DataContext = browsable
            };
        }
        
        public static void Browse<T>(ViewFactory<UniverseEditorViewModel> expression)
        {
            if (expression is null)
            {
                return;
            }

            BrowsableMappings.TryAdd(typeof(T), expression);
        }

        private static FrameworkElement GetUniversalView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (UniversalIntroduction)parameter;
            var vm = new UniversalIntroductionViewModel
            {
                Owner = owner,
                Browsable = overview
            };
            return new UniversalIntroductionView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetPropertyOverviewView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (PropertyOverview)parameter;
            var vm = new PropertyOverviewViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new PropertyOverviewView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetBrowsablePropertyView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (BrowsableProperty)parameter;
            var vm = new BrowsablePropertyViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new BrowsablePropertyView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetSpaceConceptView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (SpaceConcept)parameter;
            var vm = new SpaceConceptViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new SpaceConceptView
            {
                DataContext = vm
            };
        }
        
        
        private static FrameworkElement GetRarityConceptView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (RarityConcept)parameter;
            var vm = new RarityConceptViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new RarityConceptView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetDeclarationConceptView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (DeclarationConcept)parameter;
            var vm = new DeclarationConceptViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new DeclarationConceptView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetSpaceConceptOverviewView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (SpaceConceptOverview)parameter;
            var vm = new SpaceConceptOverviewViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new SpaceConceptOverviewView
            {
                DataContext = vm
            };
        }
        
        private static FrameworkElement GetOtherView(UniverseEditorViewModel owner, object parameter)
        {
            var overview = (OtherIntroduction)parameter;
            var vm = new OtherIntroductionViewModel
            {
                Owner     = owner,
                Browsable = overview
            };
            return new OtherIntroductionView
            {
                DataContext = vm
            };
        }
    }
}