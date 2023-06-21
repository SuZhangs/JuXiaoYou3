using System.Collections.Generic;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class MessageBoardViewModel : EntityTabViewModel<Appraise>
    {
        public MessageBoardViewModel()
        {
            ProjectEngine = Studio.Engine<ProjectEngine>();
        }

        protected sealed override bool NeedDataSourceSynchronize()
        {
            return Version != ProjectEngine.Version;
        }

        protected override void OnRequestDataSourceSynchronize(ICollection<Appraise> dataSource)
        {
            var collection = ProjectEngine.GetAppraises();
            dataSource.AddMany(collection, true);
        }

        protected override void Save()
        {
            
        }

        protected override async Task<Op<Appraise>> Add()
        {
            var source = await SubSystem.Select(DocumentType.Character);

            if (!source.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }

            var target = await SubSystem.Select(DocumentType.Character);
            
            if (!target.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }
            
            var appraise = new Appraise
            {
                Id      = ID.Get(),
                Target  = target.Value,
                Source  = source.Value,
                Content = r1.Value 
            };
            
            ProjectEngine.AddAppraise(appraise);
            return Op<Appraise>.Success(appraise);
        }

        protected override async Task Edit(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return;
            }
            
            item.Content = r1.Value;
            ProjectEngine.AddAppraise(item);
        }

        protected override void Remove(Appraise entity)
        {
            ProjectEngine.RemoveAppraise(entity);
        }

        protected override void ShiftUp(Appraise entity, int oldIndex, int newIndex)
        {
        }

        protected override void ShiftDown(Appraise entity, int oldIndex, int newIndex)
        {
        }

        protected override void ClearEntity(Appraise[] entities)
        {
            ProjectEngine.RemoveAppraise();
        }
        
        public ProjectEngine ProjectEngine { get; }
    }
}