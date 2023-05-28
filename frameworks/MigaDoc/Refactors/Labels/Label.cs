namespace Acorisoft.Miga.Doc.Labels
{
    public class Label : ObservableObject, IObjectLabel
    {
        private string _name;
        
        public string Id { get; init; }
        public string Owner { get; set; }

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