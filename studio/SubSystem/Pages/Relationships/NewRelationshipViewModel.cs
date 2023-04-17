using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
// ReSharper disable RedundantArgumentDefaultValue

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class NewRelationshipViewModel : ExplicitDialogVM
    {
        public static readonly Relationship[] Relationships = new[]
        {
            Relationship.Hostile,
            Relationship.Neutral,
            Relationship.Friendly,
            Relationship.Kinship,
            Relationship.InLaw,
        };
        
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
        private string        _nameToSource;
        private string        _nameToTarget;
        private bool          _isBidirection;
        private int           _degree;
        private Relationship  _relationship;
        
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
                NameToSource  = Entity.NameToSource;
                NameToTarget  = Entity.NameToTarget;
                Relationship  = Entity.Relationship;
                Degree        = Entity.Degree;
                IsBidirection = Entity.IsBidirection;
            }
            else
            {
                Type = (DocumentType)a[2];
            }
            base.OnStart(parameter);
        }


        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(NameToSource) &&
                   !string.IsNullOrEmpty(NameToTarget);
        }

        protected override void Finish()
        {
            if (IsEditMode)
            {
                Entity.NameToSource  = NameToSource;
                Entity.NameToTarget  = NameToTarget;
                Entity.Relationship  = Relationship;
                Entity.Degree        = Degree;
                Entity.IsBidirection = IsBidirection;
                Result               = Entity;
            }
            else
            {
                Result = new CharacterRelationship
                {
                    Id            = ID.Get(),
                    Source        = Source,
                    Target        = Target,
                    NameToSource  = NameToSource,
                    NameToTarget  = NameToTarget,
                    Degree        = Degree,
                    Relationship  = Relationship,
                    IsBidirection = IsBidirection,
                    Type = Type
                };
            }
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(NameToSource))
            {
                return SubSystemString.EmptyName;
            }
            
            if (string.IsNullOrEmpty(NameToTarget))
            {
                return SubSystemString.EmptyName;
            }

            return SubSystemString.Unknown;
        }
        
        public DocumentType Type { get; private set; }
        public bool IsEditMode { get; private set; }
        public CharacterRelationship Entity { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Relationship"/> 属性。
        /// </summary>
        public Relationship Relationship
        {
            get => _relationship;
            set => SetValue(ref _relationship, value);
        }

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

                var ns = string.IsNullOrEmpty(_nameToSource);
                var nt = string.IsNullOrEmpty(_nameToTarget);

                if (ns && nt)
                {
                    return;
                }

                if (ns)
                {
                    _nameToSource = _nameToTarget;
                    RaiseUpdated(nameof(NameToSource));
                }
                else
                {
                    
                    _nameToTarget = _nameToSource;
                    RaiseUpdated(nameof(NameToTarget));
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="NameToTarget"/> 属性。
        /// </summary>
        public string NameToTarget
        {
            get => _nameToTarget;
            set
            {
                if (IsBidirection)
                {
                    _nameToTarget = value;
                    _nameToSource = value;
                    RaiseUpdated(nameof(NameToTarget));
                    RaiseUpdated(nameof(NameToSource));
                }
                else
                {
                    SetValue(ref _nameToTarget, value);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="NameToSource"/> 属性。
        /// </summary>
        public string NameToSource
        {
            get => _nameToSource;
            set
            {
                if (IsBidirection)
                {
                    _nameToTarget = value;
                    _nameToSource = value;
                    RaiseUpdated(nameof(NameToTarget));
                    RaiseUpdated(nameof(NameToSource));
                }
                else
                {
                    SetValue(ref _nameToSource, value);
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