using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using Microsoft.CodeAnalysis;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private static void CreateProjectItems(ICollection<ProjectItemBase> collection)
        {
            collection.Clear();
            
            //
            //
            CreateWorldViewItems(collection);
            collection.Add(new ProjectSeparator());
            CreateDocumentItems(collection);
            collection.Add(new ProjectSeparator());
            CreateOtherItems(collection);
        }

        private static void CreateWorldViewItems(ICollection<ProjectItemBase> collection)
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
        
        
        private static void CreateDocumentItems(ICollection<ProjectItemBase> collection)
        {
            // 世界划分
            collection.Add(Create<FantasyProjectCharacterViewModel>("text.Project.SpaceConcept"));
            
            // 世界机制
            collection.Add(Create<FantasyProjectGeographyViewModel>("text.Project.Mechanism"));
            
            // 世界法则
            collection.Add(Create<FantasyProjectSkillViewModel>("text.Project.Rule"));
            
            // 时间轴
            collection.Add(Create<FantasyProjectItemViewModel>("text.Project.Timeline"));
        }
        
        
        private static void CreateOtherItems(ICollection<ProjectItemBase> collection)
        {
            // 世界划分
            collection.Add(Create<FantasyProjectSpaceConceptViewModel>("text.Project.SpaceConcept"));
            
            // 世界机制
            collection.Add(Create<FantasyProjectMechanismViewModel>("text.Project.Mechanism"));
            
            // 世界法则
            collection.Add(Create<FantasyProjectRuleViewModel>("text.Project.Rule"));
            
            // 时间轴
            collection.Add(Create<FantasyProjectTimelineViewModel>("text.Project.Timeline"));
            
            // 特殊系统
            collection.Add(Create<FantasyProjectTimelineViewModel>("text.Project.Timeline"));
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