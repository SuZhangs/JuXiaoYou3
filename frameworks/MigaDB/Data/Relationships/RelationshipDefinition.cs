namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    public class RelationshipDefinition : StorageUIObject
    {
        private string _callOfSource;
        private string _callOfTarget;
        private int    _friendliness;
        private bool   _isParenthood;
        private bool   _isCouple;

        /// <summary>
        /// 获取或设置 <see cref="IsCouple"/> 属性。
        /// </summary>
        public bool IsCouple
        {
            get => _isCouple;
            set => SetValue(ref _isCouple, value);
        }

        /// <summary>
        /// 是否为法律意义上的亲属关系（继父继母继兄等）
        /// </summary>
        public bool IsParenthood
        {
            get => _isParenthood;
            set => SetValue(ref _isParenthood, value);
        }

        /// <summary>
        /// 友善度
        /// </summary>
        public int Friendliness
        {
            get => _friendliness;
            set => SetValue(ref _friendliness, value);
        }

        /// <summary>
        /// 右边的称呼
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set => SetValue(ref _callOfTarget, value);
        }

        /// <summary>
        /// 左边的称呼
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set => SetValue(ref _callOfSource, value);
        }
    }
}