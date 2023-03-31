namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class Directory : StorageUIObject
    {
        private string _name;
        private string _owner;

        /// <summary>
        /// 获取或设置 <see cref="Owner"/> 属性。
        /// </summary>
        public string Owner
        {
            get => _owner;
            set => SetValue(ref _owner, value);
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