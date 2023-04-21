﻿using Acorisoft.FutureGL.MigaDB.Documents;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaDB.Data.Relationships
{
    /// <summary>
    /// 人物关系
    /// </summary>
    public class CharacterRelationship : StorageUIObject, IEdge<DocumentCache>
    {
        private string _callOfSource;
        private string _callOfTarget;
        private int    _friendliness;
        private bool   _isParenthood;
        private bool   _isCouple;
        private bool _isBiDirection;

        /// <summary>
        /// 获取或设置 <see cref="IsBiDirection"/> 属性。
        /// </summary>
        public bool IsBiDirection
        {
            get => _isBiDirection;
            set => SetValue(ref _isBiDirection, value);
        }
        
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
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }

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

        /// <summary>
        /// 源
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Source { get; init; }

        /// <summary>
        /// 目标
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Target { get; init; }
    }
}