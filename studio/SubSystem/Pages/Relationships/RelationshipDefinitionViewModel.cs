using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class RelationshipDefinitionViewModel : EntityTabViewModel<RelationshipDefinition>
    {
        protected override bool NeedDataSourceSynchronize() => true;

        protected override void OnRequestDataSourceSynchronize(ICollection<RelationshipDefinition> dataSource)
        {
            var db = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue;
            var property = db.Get<RelationshipProperty>();
            dataSource.AddMany(property.Definitions, true);
        }

        protected override void Save()
        { 
            var db = Xaml.Get<IDatabaseManager>()
                       .Database
                       .CurrentValue;
            var property = db.Get<RelationshipProperty>();
            property.Definitions.AddMany(Collection, true);
            db.Set(property);
            SetDirtyState(false);
        }

        protected override Task<Op<RelationshipDefinition>> Add()
        {
            return NewRelationshipDefinitionViewModel.New();
        }

        protected override async Task Edit(RelationshipDefinition entity)
        {
            await NewRelationshipDefinitionViewModel.Edit(entity);
        }

        protected override void Remove(RelationshipDefinition entity)
        {
            Collection.Remove(entity);
        }

        protected override void ShiftUp(RelationshipDefinition entity, int oldIndex, int newIndex)
        {
            Collection.Move(oldIndex, newIndex);
        }

        protected override void ShiftDown(RelationshipDefinition entity, int oldIndex, int newIndex)
        {
            Collection.Move(oldIndex, newIndex);
        }

        protected override void ClearEntity(RelationshipDefinition[] entities)
        {
            Collection.Clear();
        }
    }
}