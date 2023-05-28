namespace Acorisoft.Miga.Doc.Groups
{
    public enum GroupType
    {
        None,
        Carry,
        Regular,
        Skill,
        TOBSkill,
        Titled,
        Pairing
    }
    
    public class GroupingInformation : PropertyChanger
    {
        private string _name;
        private string _summary;
        private string _avatar;
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => SetValue(ref _summary, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}