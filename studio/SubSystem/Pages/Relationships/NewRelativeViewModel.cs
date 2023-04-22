using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relationships;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable RedundantArgumentDefaultValue

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relationships
{
    public class NewRelativeViewModel : ExplicitDialogVM
    {
        public static Task<Op<CharacterRelationship>> New(DocumentCache source, DocumentCache target, DocumentType type)
        {
            return DialogService().Dialog<CharacterRelationship, NewRelativeViewModel>(new Parameter
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
            return DialogService().Dialog<CharacterRelationship, NewRelativeViewModel>(new Parameter
            {
                Args = new object[]
                {
                    source,
                    target,
                    rel
                }
            });
        }

        private DocumentCache  _source;
        private DocumentCache  _target;
        private string         _callOfSource;
        private string         _callOfTarget;
        private bool           _directRelative;
        private bool           _collateralRelative;
        private bool           _conjugalRelative;
        private int            _friendliness;
        private RelativePreset _preset;

        /// <summary>
        /// 获取或设置 <see cref="Preset"/> 属性。
        /// </summary>
        public RelativePreset Preset
        {
            get => _preset;
            set
            {
                SetValue(ref _preset, value);

                if (_preset is null)
                {
                    return;
                }
                
                ConjugalRelative   = _preset.ConjugalRelative;
                DirectRelative     = _preset.DirectRelative;
                CollateralRelative = _preset.CollateralRelative;
                CallOfSource       = _preset.CallOfSource;
                CallOfTarget       = _preset.CallOfTarget;
            }
        }

        public NewRelativeViewModel()
        {
            Presets = new ObservableCollection<RelativePreset>();

            var pp = Xaml.Get<IDatabaseManager>()
                         .Database
                         .CurrentValue
                         .Get<PresetProperty>();
            var rp = pp?.RelativePresets;
            if (rp is not null) Presets.AddMany(rp);

            SwitchCommand = Command(() => { (CallOfTarget, CallOfSource) = (CallOfSource, CallOfTarget); });
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            Source     = (DocumentCache)a[0];
            Target     = (DocumentCache)a[1];
            IsEditMode = a.Length > 2 && a[2] is CharacterRelationship;

            if (IsEditMode)
            {
                Entity       = (CharacterRelationship)a[2];
                Source       = Entity.Source;
                Target       = Entity.Target;
                CallOfSource = Entity.CallOfSource;
                CallOfTarget = Entity.CallOfTarget;
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
                Entity.CallOfSource       = CallOfSource;
                Entity.CallOfTarget       = CallOfTarget;
                Entity.Friendliness       = Friendliness;
                Entity.ConjugalRelative   = ConjugalRelative;
                Entity.DirectRelative     = DirectRelative;
                Entity.CollateralRelative = CollateralRelative;
                Result                    = Entity;
            }
            else
            {
                Result = new CharacterRelationship
                {
                    Id                 = ID.Get(),
                    Source             = Source,
                    Target             = Target,
                    Friendliness       = Friendliness,
                    ConjugalRelative   = ConjugalRelative,
                    DirectRelative     = DirectRelative,
                    CollateralRelative = CollateralRelative,
                    CallOfSource       = CallOfSource,
                    CallOfTarget       = CallOfTarget,
                    Type               = Type
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
        /// 获取或设置 <see cref="Friendliness"/> 属性。
        /// </summary>
        public int Friendliness
        {
            get => _friendliness;
            set => SetValue(ref _friendliness, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfTarget"/> 属性。
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set { SetValue(ref _callOfTarget, value); }
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfSource"/> 属性。
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set { SetValue(ref _callOfSource, value); }
        }

        /// <summary>
        /// 旁系亲属
        /// </summary>
        public bool CollateralRelative
        {
            get => _collateralRelative;
            set => SetValue(ref _collateralRelative, value);
        }

        /// <summary>
        /// 夫妻关系
        /// </summary>
        public bool ConjugalRelative
        {
            get => _conjugalRelative;
            set => SetValue(ref _conjugalRelative, value);
        }

        /// <summary>
        /// 直系亲属
        /// </summary>
        public bool DirectRelative
        {
            get => _directRelative;
            set => SetValue(ref _directRelative, value);
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

        public ObservableCollection<RelativePreset> Presets { get; }
        public RelayCommand SwitchCommand { get; }
    }
}