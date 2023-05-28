namespace Acorisoft.Miga.Doc.Channels
{
    public class ChannelIndex : ObservableObject
    {

        public string Id { get; init; }
        public string Owner { get; init; }
        private string _name;
        private string _summary;

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