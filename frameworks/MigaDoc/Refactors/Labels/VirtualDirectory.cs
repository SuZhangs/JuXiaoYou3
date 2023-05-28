namespace Acorisoft.Miga.Doc.Labels
{
    public class VirtualDirectory : ObservableObject, IVirtualDirectory
    {
        private string _name;

        public string Id { get; init; }
        public string BaseOn { get; init; }

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