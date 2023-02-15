namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{

    public abstract class PropertyEditView  : ObservableObject
    {
        private string _name;
        private string _metadata;

        /// <summary>
        /// 获取或设置 <see cref="Metadata"/> 属性。
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        
        /// <summary>
        /// 完善所有的属性
        /// </summary>
        /// <returns>返回一个完整的设定属性实例。</returns>
        public abstract ModuleProperty Finish();

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        protected abstract bool IsCompleted();
        
        /// <summary>
        /// 创建模组属性。
        /// </summary>
        /// <param name="property">属性类型。</param>
        /// <returns>返回一个新的模组属性实例。</returns>
        public static PropertyEditView CreateEditView(ModuleProperty property)
        {
            // MetadataKind.(\w+)
            // $1Property
            return property switch
            {
                /*
                 * Basic
                 */
                ColorProperty Color => new ColorPropertyEditView(Color),
                DegreeProperty Degree => new DegreePropertyEditView(Degree),
                NumberProperty Number => new NumberPropertyEditView(Number),
                SliderProperty Slider => new SliderPropertyEditView(Slider),
                TextProperty Text => new TextPropertyEditView(Text),
                PageProperty Page => new PagePropertyEditView(Page),


                /*
                 * Option
                 */
                SwitchProperty Switch => new SwitchPropertyEditView(Switch),
                RadioProperty Radio => new RadioPropertyEditView(Radio),
                TalentProperty Talent => new TalentPropertyEditView(Talent),
                FavoriteProperty Favorite => new FavoritePropertyEditView(Favorite),
                BinaryProperty Binary => new BinaryPropertyEditView(Binary),
                SequenceProperty Sequence => new SequencePropertyEditView(Sequence),

                /*
                * Reference
                */
                ReferenceProperty Reference => new ReferencePropertyEditView(Reference),
                ImageProperty Image => new ImagePropertyEditView(Image),
                VideoProperty Video => new VideoPropertyEditView(Video),
                MusicProperty Music => new MusicPropertyEditView(Music),
                AudioProperty Audio => new AudioPropertyEditView(Audio),
                FileProperty File => new FilePropertyEditView(File),


                /*
                 * Chart
                 */
                HistogramProperty Histogram => new HistogramPropertyEditView(Histogram),
                RadarProperty Radar => new RadarPropertyEditView(Radar),

                /*
                 * Group
                 */
                GroupProperty Group => new GroupPropertyEditView(Group),
                RateProperty Rate => new RatePropertyEditView(Rate),
                LikabilityProperty Likability => new LikabilityPropertyEditView(Likability),

                _ => null
            };
        }
    }

    public abstract class PropertyEditView<TProperty, TFallback> : PropertyEditView, IFallbackSink<TFallback> where TProperty : ModuleProperty
    {
        private readonly TProperty _target;
        private          TFallback _fallback;

        protected PropertyEditView(TProperty property)
        {
            TargetProperty = property;
        }
        
        
        /// <summary>
        /// 构造值
        /// </summary>
        /// <param name="value">需要构造的值</param>
        /// <returns>返回目标类型。</returns>
        protected abstract string ConstructFallback(TFallback value);
        
        /// <summary>
        /// 解构值
        /// </summary>
        /// <param name="value">需要解构的值</param>
        /// <returns>返回目标类型。</returns>
        protected abstract TFallback DeconstructFallback(string value);

        /// <summary>
        /// 完善所有的属性
        /// </summary>
        /// <returns>返回一个完整的设定属性实例。</returns>
        protected abstract TProperty FinishState();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override ModuleProperty Finish()
        {
            return FinishState();
        }
        

        /// <summary>
        /// 目标属性。
        /// </summary>
        public TProperty TargetProperty
        {
            get
            {
                return _target;
            }
            
            private init
            {
                _target  = value;
                Name     = _target.Name;
                Metadata = _target.Metadata;
                Fallback = DeconstructFallback(_target.Value);
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Fallback"/> 属性。
        /// </summary>
        public TFallback Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }
        
        


    }
}