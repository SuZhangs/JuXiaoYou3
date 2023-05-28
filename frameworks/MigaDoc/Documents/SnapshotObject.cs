namespace Acorisoft.Miga.Doc.Documents
{
    public abstract class SnapshotObject : ObservableObject
    {
        private string _name;
        
        /// <summary>
        /// 获取或设置 <see cref="Id"/> 属性。
        /// </summary>
        public string Id { get; init; }

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