namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class Contract : NotifyPropertyChanged
    {
        private string _groupName;

        /// <summary>
        /// 获取或设置 <see cref="GroupName"/> 属性。
        /// </summary>
        public string GroupName
        {
            get => _groupName;
            set => SetValue(ref _groupName, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Character"/> 属性。
        /// </summary>
        [BsonRef(Constants.Name_Chat_Character)]
        public Character Character { get; init; }
    }
}