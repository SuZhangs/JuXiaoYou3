using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public abstract class PropertyDataView : ObservableObject
    {
        protected void Notification(string newValue, string metadata, bool fallback)
        {
            // TODO: notification and clamp
        }
        
        


        /// <summary>
        /// 创建模组属性。
        /// </summary>
        /// <param name="property">属性类型。</param>
        /// <returns>返回一个新的模组属性实例。</returns>
        public static PropertyDataView Create(ModuleProperty property)
        {
            // MetadataKind.(\w+)
            // $1Property
            return property switch
            {
                /*
                 * Basic
                 */
                ColorProperty Color => new ColorPropertyDataView(Color),
                DegreeProperty Degree => new DegreePropertyDataView(Degree),
                NumberProperty Number => new NumberPropertyDataView(Number),
                SliderProperty Slider => new SliderPropertyDataView(Slider),
                TextProperty Text => new TextPropertyDataView(Text),
                PageProperty Page => new PagePropertyDataView(Page),


                /*
                 * Option
                 */
                SwitchProperty Switch => new SwitchPropertyDataView(Switch),
                RadioProperty Radio => new RadioPropertyDataView(Radio),
                TalentProperty Talent => new TalentPropertyDataView(Talent),
                FavoriteProperty Favorite => new FavoritePropertyDataView(Favorite),
                BinaryProperty Binary => new BinaryPropertyDataView(Binary),
                SequenceProperty Sequence => new SequencePropertyDataView(Sequence),

                /*
                * Reference
                */
                ReferenceProperty Reference => new ReferencePropertyDataView(Reference),
                ImageProperty Image => new ImagePropertyDataView(Image),
                VideoProperty Video => new VideoPropertyDataView(Video),
                MusicProperty Music => new MusicPropertyDataView(Music),
                AudioProperty Audio => new AudioPropertyDataView(Audio),
                FileProperty File => new FilePropertyDataView(File),


                /*
                 * Chart
                 */
                HistogramProperty Histogram => new HistogramPropertyDataView(Histogram),
                RadarProperty Radar => new RadarPropertyDataView(Radar),

                /*
                 * Group
                 */
                GroupProperty Group => new GroupPropertyDataView(Group),
                RateProperty Rate => new RatePropertyDataView(Rate),
                LikabilityProperty Likability => new LikabilityPropertyDataView(Likability),

                _ => null
            };
        }
    }

    public abstract class PropertyDataView<TProperty, TValue> : PropertyDataView where TProperty : ModuleProperty
    {
        private readonly TProperty _target;

        private TValue _value;
        private string _name;
        private string _metadata;

        protected PropertyDataView(TProperty property)
        {
            TargetProperty = property;
        }

        /// <summary>
        /// 构造值
        /// </summary>
        /// <param name="value">需要构造的值</param>
        /// <returns>返回目标类型。</returns>
        protected abstract string GenericValueToString(TValue value);

        /// <summary>
        /// 解构值
        /// </summary>
        /// <param name="value">需要解构的值</param>
        /// <returns>返回目标类型。</returns>
        protected abstract TValue StringToGenericValue(string value);

        /// <summary>
        /// 当值发生改变时，应该在此完成操作。
        /// </summary>
        /// <param name="oldValue">旧的值</param>
        /// <param name="newValue">新的值</param>
        /// <param name="fallback">返回改变状态，如果为true则通知属性变更。</param>
        /// <returns>新的值</returns>
        protected abstract TValue OnValueChanged(TValue oldValue, TValue newValue, out bool fallback);

        /// <summary>
        /// 获取默认值或者回滚。
        /// </summary>
        /// <returns>返回默认值或者回滚</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string GetValueOrFallback() => string.IsNullOrEmpty(_target.Value) ? _target.Fallback : _target.Value;

        /// <summary>
        /// 获取或设置 <see cref="Metadata"/> 属性。
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            private set => SetValue(ref _metadata, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            private set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 目标属性。
        /// </summary>
        public TProperty TargetProperty
        {
            get { return _target; }

            private init
            {
                _target   = value;
                _value    = StringToGenericValue(GetValueOrFallback());
                _name     = _target.Name;
                _metadata = _target.Metadata;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public TValue Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<TValue>.Default.Equals(_value, value))
                {
                    return;
                }

                var newValue = OnValueChanged(_value, value, out var result);
                _value        = newValue;
                _target.Value = GenericValueToString(newValue);
                RaiseUpdated();
                Notification(_target.Value, _metadata, result);
            }
        }
    }
}