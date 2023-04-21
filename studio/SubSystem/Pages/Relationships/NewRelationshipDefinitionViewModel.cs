﻿using Acorisoft.FutureGL.MigaDB.Data.Relationships;

// ReSharper disable RedundantArgumentDefaultValue

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class NewRelationshipDefinitionViewModel : ExplicitDialogVM
    {
        public static Task<Op<RelationshipDefinition>> New()
        {
            return DialogService().Dialog<RelationshipDefinition, NewRelationshipDefinitionViewModel>();
        }

        public static Task<Op<RelationshipDefinition>> Edit(RelationshipDefinition rel)
        {
            return DialogService().Dialog<RelationshipDefinition, NewRelationshipDefinitionViewModel>(new Parameter
            {
                Args = new object[]
                {
                    rel
                }
            });
        }


        private string _callOfSource;
        private string _callOfTarget;
        private bool   _isBidirection;
        private int    _friendliness;
        private bool   _isParenthood;
        private bool   _isCouple;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            IsEditMode = a.Length > 0 && a[0] is RelationshipDefinition;

            if (IsEditMode)
            {
                Entity       = (RelationshipDefinition)a[0];
                CallOfSource = Entity.CallOfSource;
                CallOfTarget = Entity.CallOfTarget;
            }

            base.OnStart(parameter);
        }


        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(CallOfSource) &&
                   !string.IsNullOrEmpty(CallOfTarget);
        }

        protected override void Finish()
        {
            if (IsEditMode)
            {
                Entity.CallOfSource = CallOfSource;
                Entity.CallOfTarget = CallOfTarget;
                Entity.IsParenthood = IsParenthood;
                Entity.IsCouple     = IsCouple;
                Entity.Friendliness = Friendliness;
                Result              = Entity;
            }
            else
            {
                Result = new RelationshipDefinition
                {
                    Id           = ID.Get(),
                    CallOfSource = CallOfSource,
                    CallOfTarget = CallOfTarget,
                    IsCouple     = IsCouple,
                    IsParenthood = IsParenthood,
                    Friendliness = Friendliness
                };
            }
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(CallOfSource))
            {
                return SubSystemString.EmptyName;
            }

            if (string.IsNullOrEmpty(CallOfTarget))
            {
                return SubSystemString.EmptyName;
            }

            return SubSystemString.Unknown;
        }

        public DocumentType Type { get; private set; }
        public bool IsEditMode { get; private set; }
        public RelationshipDefinition Entity { get; private set; }

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
        /// 获取或设置 <see cref="Friendliness"/> 属性。
        /// </summary>
        public int Friendliness
        {
            get => _friendliness;
            set => SetValue(ref _friendliness, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsBidirection"/> 属性。
        /// </summary>
        public bool IsBidirection
        {
            get => _isBidirection;
            set
            {
                SetValue(ref _isBidirection, value);

                var ns = string.IsNullOrEmpty(_callOfSource);
                var nt = string.IsNullOrEmpty(_callOfTarget);

                if (ns && nt)
                {
                    return;
                }

                if (ns)
                {
                    _callOfSource = _callOfTarget;
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    _callOfTarget = _callOfSource;
                    RaiseUpdated(nameof(CallOfTarget));
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfTarget"/> 属性。
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set
            {
                if (IsBidirection)
                {
                    _callOfTarget = value;
                    _callOfSource = value;
                    RaiseUpdated(nameof(CallOfTarget));
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    SetValue(ref _callOfTarget, value);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfSource"/> 属性。
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set
            {
                if (IsBidirection)
                {
                    _callOfTarget = value;
                    _callOfSource = value;
                    RaiseUpdated(nameof(CallOfTarget));
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    SetValue(ref _callOfSource, value);
                }
            }
        }
    }
}