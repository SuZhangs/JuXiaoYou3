using Acorisoft.FutureGL.MigaDB.Data.Relationships;
// ReSharper disable RedundantArgumentDefaultValue

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class NewRelationshipViewModel : ExplicitDialogVM
    {
        public static Task<Op<CharacterRelationship>> New(DocumentCache source, DocumentCache target, DocumentType type)
        {
            return DialogService().Dialog<CharacterRelationship, NewRelationshipViewModel>(new Parameter
            {
                Args = new object[]
                {
                    source,
                    target,
                    type
                }
            });
        }
        
        public static Task<Op<CharacterRelationship>> New(DocumentCache source, DocumentCache target, CharacterRelationship rel)
        {
            return DialogService().Dialog<CharacterRelationship, NewRelationshipViewModel>(new Parameter
            {
                Args = new object[]
                {
                    source,
                    target,
                    rel
                }
            });
        }

        private DocumentCache _source;
        private DocumentCache _target;
        private string        _callOfSource;
        private string        _callOfTarget;
        private bool          _isBidirection;
        private int           _degree;
        
        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            Source     = (DocumentCache)a[0];
            Target     = (DocumentCache)a[1];
            IsEditMode = a.Length > 2 && a[2] is CharacterRelationship;

            if (IsEditMode)
            {
                Entity        = (CharacterRelationship)a[2];
                Source        = Entity.Source;
                Target        = Entity.Target;
                CallOfSource  = Entity.CallOfSource;
                CallOfTarget  = Entity.CallOfTarget;
            }
            else
            {
                Type = (DocumentType)a[2];
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
                Entity.CallOfSource  = CallOfSource;
                Entity.CallOfTarget  = CallOfTarget;
                Result               = Entity;
            }
            else
            {
                Result = new CharacterRelationship
                {
                    Id            = ID.Get(),
                    Source        = Source,
                    Target        = Target,
                    CallOfSource  = CallOfSource,
                    CallOfTarget  = CallOfTarget,
                    Type          = Type
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
        public CharacterRelationship Entity { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Degree"/> 属性。
        /// </summary>
        public int Degree
        {
            get => _degree;
            set => SetValue(ref _degree, value);
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

        /// <summary>
        /// 获取或设置 <see cref="Target"/> 属性。
        /// </summary>
        public DocumentCache Target
        {
            get => _target;
            set => SetValue(ref _target, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Source"/> 属性。
        /// </summary>
        public DocumentCache Source
        {
            get => _source;
            set => SetValue(ref _source, value);
        }
    }
}