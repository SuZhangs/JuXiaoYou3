using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using Acorisoft.FutureGL.MigaUtils.Collections;

// ReSharper disable SuggestBaseTypeForParameter

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    partial class UniverseViewModel
    {
        
        private async Task AddTimelineConceptImpl(bool isAge, TimelineConcept target, bool after)
        {
            TimelineConcept concept;

            if (isAge)
            {
                var r = await NewTimelineViewModel.NewAge();

                if (!r.IsFinished)
                {
                    return;
                }

                concept = r.Value;
            }
            else
            {
                var r = await NewTimelineViewModel.NewEvent();

                if (!r.IsFinished)
                {
                    return;
                }

                concept = r.Value;
            }

            if (Timelines.Count == 0)
            {
                //
                // add first node
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }

            if (target is null)
            {
                var last = Timelines.Last();
                last.NextItem = concept.Id;
                ProjectEngine.AddTimeline(last);
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }
            
            var index = Timelines.IndexOf(target);

            if (index == -1)
            {
                var last = Timelines.Last();
                last.NextItem = concept.Id;
                ProjectEngine.AddTimeline(last);
                ProjectEngine.AddTimeline(concept);
                Timelines.Add(concept);
                return;
            }

            if (after)
            {
                if (index == Timelines.Count - 1)
                {
                    /*
                     *  A
                     *  |
                     *  target
                     *  | < ----- concept
                     *  |
                     *  null
                     */

                    concept.LastItem = target.Id;
                    target.NextItem  = concept.Id;
                    ProjectEngine.AddTimeline(target);
                    ProjectEngine.AddTimeline(concept);
                }
                else
                {
                    /*
                     *  A
                     *  |
                     *  target
                     *  | < ----- concept
                     *  |
                     *  B
                     */
                    var b = Timelines[index + 1];
                    concept.NextItem = b.LastItem;
                    concept.LastItem = target.Id;
                    target.NextItem  = concept.Id;
                    b.LastItem       = concept.Id;
                    ProjectEngine.AddTimeline(target);
                    ProjectEngine.AddTimeline(concept);
                    ProjectEngine.AddTimeline(b);
                }
                Timelines.Add(concept);
                return;
            }
            
            if (index == 0)
            {
                /*
                 *  null
                 *  |
                 *  | < ----- concept
                 *  target
                 *  |
                 *  b
                 */

                concept.NextItem = target.Id;
                target.LastItem  = concept.Id;
                ProjectEngine.AddTimeline(target);
                ProjectEngine.AddTimeline(concept);
            }
            else
            {
                /*
                 *  a
                 *  |
                 *  | < ----- concept
                 *  target
                 *  |
                 *  b
                 */
                var a = Timelines[index - 1];

                concept.NextItem = target.Id;
                concept.LastItem = a.Id;
                a.NextItem       = concept.Id;
                target.LastItem  = concept.Id;
                ProjectEngine.AddTimeline(target);
                ProjectEngine.AddTimeline(a);
                ProjectEngine.AddTimeline(concept);
            }
            Timelines.Add(concept);
        }

        private Task AddTimelineAgeImpl()
        {
            return AddTimelineConceptImpl(true, null, true);
        }
        
        private Task AddTimelineAgeBeforeImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(true, target, false);
        }
        
        private Task AddTimelineAgeAfterImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(true, target, true);
        }
        
        private Task AddTimelineEventBeforeImpl(TimelineConcept target)
        { 
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            return AddTimelineConceptImpl(true, target, false);
        }
        
        private Task AddTimelineEventAfterImpl(TimelineConcept target)
        {
            if (target is null)
            {
                return Task.FromException(new ArgumentNullException(nameof(target)));
            }
            
            return AddTimelineConceptImpl(true, target, true);
        }
        
        private Task AddTimelineEventImpl()
        {
            return AddTimelineConceptImpl(false, null, true);
        }
        
        private void ShiftDownTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }

            var index = Timelines.IndexOf(concept);

            if (index == -1)
            {
                return;
            }

            if (index == Timelines.Count - 1)
            {
                return;
            }
            
            var next = Timelines[index + 1];
            next.LastItem    = concept.LastItem;
            concept.NextItem = next.NextItem;
            concept.LastItem = next.Id;
            next.NextItem    = concept.Id;
            Timelines.ShiftDown(concept);
            ProjectEngine.AddTimeline(concept);
            ProjectEngine.AddTimeline(next);
        }
        
        private void ShiftUpTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }

            var index = Timelines.IndexOf(concept);

            if (index == -1)
            {
                return;
            }

            if (index == 0)
            {
                return;
            }

            var last = Timelines[index - 1];
            last.LastItem    = concept.LastItem;
            concept.NextItem = last.NextItem;
            concept.LastItem = last.Id;
            last.NextItem    = concept.Id;
            
            Timelines.ShiftUp(concept);
            ProjectEngine.AddTimeline(concept);
            ProjectEngine.AddTimeline(last);
        }

        private async Task RemoveTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }
            
            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }


            var index = Timelines.IndexOf(concept);

            if (index == -1)
            {
                return;
            }

            if (Timelines.Count == 1)
            {
                // only-one
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            TimelineConcept last;
            TimelineConcept next;
            
            if (index == 0)
            {
                // first
                next = Timelines[1];
                next.LastItem = null;
                ProjectEngine.AddTimeline(next);
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            if (index == Timelines.Count - 1)
            {
                // last
                last          = Timelines[index - 1];
                last.NextItem = null;

                ProjectEngine.AddTimeline(last);
                ProjectEngine.RemoveTimeline(concept);
                Timelines.Remove(concept);
                return;
            }

            last = Timelines[index - 1];
            next = Timelines[index + 1];

            last.NextItem = next.Id;
            next.LastItem = last.Id;
                
                
            ProjectEngine.AddTimeline(last);
            ProjectEngine.AddTimeline(next);
            ProjectEngine.RemoveTimeline(concept);
            Timelines.Remove(concept);
            
        }
        
        private async Task EditTimelineImpl(TimelineConcept concept)
        {
            if (concept is null)
            {
                return;
            }

            var r = await NewTimelineViewModel.Edit(concept);

            if (!r.IsFinished)
            {
                return;
            }
            
            ProjectEngine.AddTimeline(r.Value);
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTimelineAgeCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineAgeAfterCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineAgeBeforeCommand { get; }
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddTimelineEventCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineEventAfterCommand { get; }
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> AddTimelineEventBeforeCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TimelineConcept> ShiftUpTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TimelineConcept> ShiftDownTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> EditTimelineCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TimelineConcept> RemoveTimelineCommand { get; }
        public ObservableCollection<TimelineConcept> Timelines { get; }
    }
}