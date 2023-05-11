using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Universe;
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

        private static UserControl GetUniversalView(UniverseEditorViewModel owner, object parameter)
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
        
        private static UserControl GetPropertyOverviewView(UniverseEditorViewModel owner, object parameter)
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
        
        private static UserControl GetBrowsablePropertyView(UniverseEditorViewModel owner, object parameter)
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
        
        private static UserControl GetSpaceConceptView(UniverseEditorViewModel owner, object parameter)
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
        
        
        
        private static UserControl GetSpaceConceptOverviewView(UniverseEditorViewModel owner, object parameter)
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
        
        private static UserControl GetOtherView(UniverseEditorViewModel owner, object parameter)
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