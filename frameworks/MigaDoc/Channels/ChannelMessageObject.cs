namespace Acorisoft.Miga.Doc.Channels
{
    public class ChannelMessageObject : ObservableObject
    {
        private bool _isOwner;

        /// <summary>
        /// 获取或设置 <see cref="IsOwner"/> 属性。
        /// </summary>
        public bool IsOwner
        {
            get => _isOwner;
            set => SetValue(ref _isOwner, value);
        }
        public string Id { get; init; }
        public string Avatar { get; init; }
        public string Name { get; init; }
        public string Message { get; init; }
    }
}