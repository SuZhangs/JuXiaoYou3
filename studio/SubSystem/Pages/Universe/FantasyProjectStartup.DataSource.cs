using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using Microsoft.CodeAnalysis;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItem _spaceRoot;

        private void CreateProjectItems()
        {
            ProjectElements.Clear();
            DocumentElements.Clear();
            OtherElements.Clear();

            //
            //
            CreateWorldViewItems(ProjectElements);
            CreateDocumentItems(DocumentElements);
            CreateOtherItems(OtherElements);
        }

        private void CreateWorldViewItems(ICollection<ProjectItem> collection)
        {
            // 世界划分
            CreateSpaceConcept(collection);

            // 世界机制
            collection.Add(Create<FantasyProjectMechanismViewModel>("text.Project.Mechanism"));

            // 世界法则
            collection.Add(Create<FantasyProjectRuleViewModel>("text.Project.Rule"));

            // 时间轴
            collection.Add(Create<FantasyProjectTimelineViewModel>("text.Project.Timeline"));
        }

        private void CreateSpaceConcept(ICollection<ProjectItem> collection)
        {
            _spaceRoot = Create<FantasyProjectSpaceConceptViewModel>("text.Project.SpaceConcept");

            

            collection.Add(_spaceRoot);
        }


        private static void CreateDocumentItems(ICollection<ProjectItem> collection)
        {
            // 人物
            collection.Add(CreateCharacters());

            // 地理
            collection.Add(Create<FantasyProjectGeographyViewModel>("text.Project.Geography"));

            // 能力
            collection.Add(Create<FantasyProjectSkillViewModel>("text.Project.Skill"));

            // 物品
            collection.Add(CreateItems());

            // 政治团体
            collection.Add(CreatePoliticalBlocs());

            // 知识
            collection.Add(CreateKnowledge());
        }


        private static void CreateOtherItems(ICollection<ProjectItem> collection)
        {
            // 其他
            collection.Add(Create<FantasyProjectOtherViewModel>("text.Project.Other"));

            // 关系
            collection.Add(Create<FantasyProjectRelativesViewModel>("text.Project.Relatives"));
        }

        private static ProjectItem CreatePoliticalBlocs()
        {
            var parent = Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs");
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Country", PoliticalBlocs.Country));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Force", PoliticalBlocs.Force));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Organization", PoliticalBlocs.Organization));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Tribe", PoliticalBlocs.Tribe));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Clan", PoliticalBlocs.Clan));
            return parent;
        }

        private static ProjectItem CreateCharacters()
        {
            var parent = Create<FantasyProjectCharacterViewModel>("text.Project.Character");
            parent.Children
                  .Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.All", Character.All));
            parent.Children
                  .Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Primary", Character.Primary));
            parent.Children
                  .Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Secondary", Character.Secondary));
            parent.Children
                  .Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.NPC", Character.NPC));
            parent.Children
                  .Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Binding", Character.Binding));
            return parent;
        }

        private static ProjectItem CreateItems()
        {
            var parent = Create<FantasyProjectItemViewModel>("text.Project.Item");
            parent.Children
                  .Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Product", ItemType.Product));
            parent.Children
                  .Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Artifact", ItemType.Artifact));
            parent.Children
                  .Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Material", ItemType.Material));
            parent.Children
                  .Add(Create<FantasyProjectItemViewModel>("text.Project.Item.UnprocessedMaterial", ItemType.UnprocessedMaterial));
            parent.Children
                  .Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Equipment", ItemType.Equipment));
            return parent;
        }

        private static ProjectItem CreateKnowledge()
        {
            var parent = Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge");
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Planets", KnowledgeType.Planets));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Creatures", KnowledgeType.Creatures));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Gods", KnowledgeType.Gods));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Devils", KnowledgeType.Devils));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Poison", KnowledgeType.Poison));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Calamity", KnowledgeType.Calamity));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Religion", KnowledgeType.Religion));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Thread", KnowledgeType.Thread));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Class", KnowledgeType.Class));
            parent.Children
                  .Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Tale", KnowledgeType.Tale));
            return parent;
        }

        private static ProjectItem Create<TViewModel>(string id) where TViewModel : TabViewModel
        {
            return new ProjectItem
            {
                Name      = Language.GetText(id),
                ViewModel = Xaml.GetViewModel<TViewModel>(),
                Children  = new ObservableCollection<ProjectItem>(),
                Property  = new ObservableCollection<ProjectItem>(),
            };
        }

        private static ProjectItem Create<TViewModel>(string id, object type) where TViewModel : TabViewModel
        {
            return new ProjectItem
            {
                Name       = Language.GetText(id),
                ViewModel  = Xaml.GetViewModel<TViewModel>(),
                Parameter1 = type,
                Children   = new ObservableCollection<ProjectItem>(),
                Property   = new ObservableCollection<ProjectItem>(),
            };
        }
    }
}