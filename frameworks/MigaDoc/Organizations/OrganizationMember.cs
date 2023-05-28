namespace Acorisoft.Miga.Doc.Entities.Organizations
{
    public class OrganizationMember : PropertyChanger
    {
        public string Id { get; init; }
        public string Avatar { get; init; }
        public string Name { get; init; }
        private string _summary;

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => SetValue(ref _summary, value);
        }
    }
}