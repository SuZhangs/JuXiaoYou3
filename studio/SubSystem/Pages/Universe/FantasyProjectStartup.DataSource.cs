using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using Microsoft.CodeAnalysis;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class FantasyProjectStartupViewModel : TabViewModel
    {
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

        private static void CreateWorldViewItems(ICollection<ProjectItem> collection)
        {
            // 世界划分
            collection.Add(Create<FantasyProjectSpaceConceptViewModel>("text.Project.SpaceConcept"));

            // 世界机制
            collection.Add(Create<FantasyProjectMechanismViewModel>("text.Project.Mechanism"));

            // 世界法则
            collection.Add(Create<FantasyProjectRuleViewModel>("text.Project.Rule"));

            // 时间轴
            collection.Add(Create<FantasyProjectTimelineViewModel>("text.Project.Timeline"));
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
            // 世界划分
            collection.Add(Create<FantasyProjectSpaceConceptViewModel>("text.Project.SpaceConcept"));

            // 世界机制
            collection.Add(Create<FantasyProjectMechanismViewModel>("text.Project.Mechanism"));
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
            var parent = Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character");
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character.All", Character.All));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character.Primary", Character.Primary));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character.Secondary", Character.Secondary));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character.NPC", Character.NPC));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Character.Binding", Character.Binding));
            return parent;
        }

        private static ProjectItem CreateItems()
        {
            var parent = Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item");
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item.Product", ItemType.Product));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item.Artifact", ItemType.Artifact));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item.Material", ItemType.Material));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item.UnprocessedMaterial", ItemType.UnprocessedMaterial));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Item.Equipment", ItemType.Equipment));
            return parent;
        }

        private static ProjectItem CreateKnowledge()
        {
            var parent = Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge");
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Planets", KnowledgeType.Planets));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Creatures", KnowledgeType.Creatures));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Gods", KnowledgeType.Gods));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Devils", KnowledgeType.Devils));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Poison", KnowledgeType.Poison));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Calamity", KnowledgeType.Calamity));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Religion", KnowledgeType.Religion));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Thread", KnowledgeType.Thread));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Class", KnowledgeType.Class));
            parent.Children
                  .Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.Knowledge.Tale", KnowledgeType.Tale));
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

        /*
         *
text.Project.Overview = 总览
text.Project.Overview.Space = 世界
text.Project.Overview.Document = 设定
text.Project.Overview.Rule = 法则
text.Project.Overview.Other = 其他
text.Project.Rarity = 稀有度
text.Project.Declaration = 词条
text.Project.Property.NPC = NPC
text.Project.Property.Country = 国家
text.Project.Property.Gangbang = 势力
text.Project.Property.Team = 团队
text.Project.Property.Planets = 植物
text.Project.Property.Creatures = 生物
text.Project.Property.Material = 素材
text.Project.Property.Ore = 矿物
text.Project.Property.Gods = 神祇
text.Project.Property.Devils = 恶魔
text.Project.Property.Technology = 科技
text.Project.Property.Elemental = 元素
text.Project.Property.Poison = 毒物
text.Project.Property.Calamity = 灾厄
text.Project.Property.Magic = 魔法
text.Project.Property.Physic = 物理
         */
    }
}