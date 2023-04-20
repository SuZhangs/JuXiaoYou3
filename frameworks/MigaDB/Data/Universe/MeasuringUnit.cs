namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public class MeasuringUnit : StorageUIObject
    {
        private string _name;

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