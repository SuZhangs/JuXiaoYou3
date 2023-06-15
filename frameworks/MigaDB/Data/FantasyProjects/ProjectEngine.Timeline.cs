using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {

        #region Timelines

        public void AddTimeline(TimelineConcept item)
        {
            if (item is null)
            {
                return;
            }

#if DEBUG
            Debug.WriteLine($"last:{item.LastItem}\ncurrent:{item.Id}\nnext:{item.NextItem}\n");
#endif

            Timelines.Upsert(item);
        }

        public void RemoveTimeline(TimelineConcept item)
        {
            if (item is null)
            {
                return;
            }

            Timelines.Delete(item.Id);
        }

        public IEnumerable<TimelineConcept> GetTimelines() => Timelines.FindAll();

        #endregion
        
        

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<TimelineConcept> Timelines { get; private set; }
    }
}