using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
     public class CharacterData : DocumentData
    {
        public SkillData Skills { get; set; }
        public ObservableCollection<DocumentIndex> SkillData { get; set; }
        public ObservableCollection<string> MainTheme { get; set; }
        public ObservableCollection<object> StoryBoard { get; set; }
        public ObservableCollection<Relationship> Relationships { get; set; }
        public ObservableCollection<string> Lines { get; set; }
    }

    public class SkillData : ObservableObject
    {
        private DocumentIndex _passive;
        private DocumentIndex _cost;
        private DocumentIndex _skill1;
        private DocumentIndex _skill2;
        private DocumentIndex _skill3;
        private DocumentIndex _skill4;

        /// <summary>
        /// 获取或设置 <see cref="Skill4"/> 属性。
        /// </summary>
        public DocumentIndex Skill4
        {
            get => _skill4;
            set => SetValue(ref _skill4, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Skill3"/> 属性。
        /// </summary>
        public DocumentIndex Skill3
        {
            get => _skill3;
            set => SetValue(ref _skill3, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Skill2"/> 属性。
        /// </summary>
        public DocumentIndex Skill2
        {
            get => _skill2;
            set => SetValue(ref _skill2, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Skill1"/> 属性。
        /// </summary>
        public DocumentIndex Skill1
        {
            get => _skill1;
            set => SetValue(ref _skill1, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Cost"/> 属性。
        /// </summary>
        public DocumentIndex Cost
        {
            get => _cost;
            set => SetValue(ref _cost, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Passive"/> 属性。
        /// </summary>
        public DocumentIndex Passive
        {
            get => _passive;
            set => SetValue(ref _passive, value);
        }
    }
}