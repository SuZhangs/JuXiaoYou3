namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Module
{
    public class OptionItem : ObservableObject
    {
        private string _name;
        private string _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
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