﻿using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {
        

        #region Appraise

        public void AddAppraise(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            AppraiseDB.Upsert(item);
        }

        public void RemoveAppraise(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            AppraiseDB.Delete(item.Id);
        }
        
        public void RemoveAppraise()
        {
            AppraiseDB.DeleteAll();
        }

        public IEnumerable<Appraise> GetAppraises(DocumentCache source)
        {
            if (source is null)
            {
                return GetAppraises();
            }

            return AppraiseDB
                   .Include(y => y.Source)
                   .Include(a => a.Target)
                   .Find(x => x.Target.Id == source.Id)
                   .Where(x => x.Target is not null &&
                               x.Source is not null &&
                               !x.Target.IsDeleted &&
                               !x.Source.IsDeleted);
        }

        public IEnumerable<Appraise> GetAppraises() => AppraiseDB.Include(y => y.Source)
                                                                 .Include(a => a.Target)
                                                                 .FindAll();

        #endregion

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Appraise> AppraiseDB { get; private set; }
    }
}