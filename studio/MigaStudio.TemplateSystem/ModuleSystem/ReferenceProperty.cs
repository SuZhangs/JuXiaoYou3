namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public abstract class TargetPropertyDataView<TProperty> : PropertyDataView, IBindableTargetSink where TProperty : TargetProperty
    {
        private readonly TProperty _target;

        private string _name;
        private string _metadata;

        protected TargetPropertyDataView(TProperty property)
        {
            TargetProperty = property;
        }


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
                _target       = value;
                _name         = _target.Name;
                _metadata     = _target.Metadata;
                DisplayName   = _target.DisplayName;
                DisplaySource = _target.DisplaySource;
                Thumbnail     = _target.Thumbnail;
            }
        }

        public string DisplaySource
        {
            get => _target.DisplaySource;
            set
            {
                _target.DisplaySource = value;
                RaiseUpdated();
            }
        }

        public string DisplayName
        {
            get => _target.DisplayName;
            set
            {
                _target.DisplayName = value;
                RaiseUpdated();
            }
        }

        public string Thumbnail
        {
            get => _target.Thumbnail;
            set
            {
                _target.Thumbnail = value;
                RaiseUpdated();
            }
        }
    }

    public abstract class TargetPropertyEditView<TProperty> : PropertyEditView where TProperty : TargetProperty
    {
        private readonly TProperty _target;

        protected TargetPropertyEditView(TProperty property)
        {
            TargetProperty = property;
        }


        /// <summary>
        /// 完善所有的属性
        /// </summary>
        /// <returns>返回一个完整的设定属性实例。</returns>
        public sealed override ModuleProperty Finish()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name);


        /// <summary>
        /// 目标属性。
        /// </summary>
        public TProperty TargetProperty
        {
            get { return _target; }

            private init
            {
                _target  = value;
                Name     = _target.Name;
                Metadata = _target.Metadata;
            }
        }
    }

    public class ReferencePropertyDataView : TargetPropertyDataView<ReferenceProperty>
    {
        public ReferencePropertyDataView(ReferenceProperty property) : base(property)
        {
        }
    }

    public class ReferencePropertyEditView : TargetPropertyEditView<ReferenceProperty>
    {
        public ReferencePropertyEditView(ReferenceProperty property) : base(property)
        {
        }
    }

    public class AudioPropertyDataView : TargetPropertyDataView<AudioProperty>
    {
        public AudioPropertyDataView(AudioProperty property) : base(property)
        {
        }
    }

    public class AudioPropertyEditView : TargetPropertyEditView<AudioProperty>
    {
        public AudioPropertyEditView(AudioProperty property) : base(property)
        {
        }
    }

    public class MusicPropertyDataView : TargetPropertyDataView<MusicProperty>
    {
        public MusicPropertyDataView(MusicProperty property) : base(property)
        {
        }
    }

    public class MusicPropertyEditView : TargetPropertyEditView<MusicProperty>
    {
        public MusicPropertyEditView(MusicProperty property) : base(property)
        {
        }
    }

    public class ImagePropertyDataView : TargetPropertyDataView<ImageProperty>
    {
        public ImagePropertyDataView(ImageProperty property) : base(property)
        {
        }
    }

    public class ImagePropertyEditView : TargetPropertyEditView<ImageProperty>
    {
        public ImagePropertyEditView(ImageProperty property) : base(property)
        {
        }
    }

    public class VideoPropertyDataView : TargetPropertyDataView<VideoProperty>
    {
        public VideoPropertyDataView(VideoProperty property) : base(property)
        {
        }
    }

    public class VideoPropertyEditView : TargetPropertyEditView<VideoProperty>
    {
        public VideoPropertyEditView(VideoProperty property) : base(property)
        {
        }
    }

    public class FilePropertyDataView : TargetPropertyDataView<FileProperty>
    {
        public FilePropertyDataView(FileProperty property) : base(property)
        {
        }
    }

    public class FilePropertyEditView : TargetPropertyEditView<FileProperty>
    {
        public FilePropertyEditView(FileProperty property) : base(property)
        {
        }
    }
}