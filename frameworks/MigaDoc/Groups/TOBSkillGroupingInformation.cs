namespace Acorisoft.Miga.Doc.Groups
{
    public class TOBSkillGroupingInformation : GroupingInformation
    {
        private DocumentIndexCopy _passive;
        private DocumentIndexCopy _cost;
        private DocumentIndexCopy _skill1;
        private DocumentIndexCopy _skill2;
        private DocumentIndexCopy _skill3;
        private DocumentIndexCopy _skill4;

        /// <summary>
        /// 获取或设置 <see cref="Skill4"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill4
        {
            get => _skill4;
            set => SetValue(ref _skill4, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Skill3"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill3
        {
            get => _skill3;
            set => SetValue(ref _skill3, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Skill2"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill2
        {
            get => _skill2;
            set => SetValue(ref _skill2, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Skill1"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill1
        {
            get => _skill1;
            set => SetValue(ref _skill1, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Cost"/> 属性。
        /// </summary>
        public DocumentIndexCopy Cost
        {
            get => _cost;
            set => SetValue(ref _cost, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Passive"/> 属性。
        /// </summary>
        public DocumentIndexCopy Passive
        {
            get => _passive;
            set => SetValue(ref _passive, value);
        }

    }
}